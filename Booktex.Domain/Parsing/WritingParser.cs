using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Booktex.Domain.Parsing; 
public static class WritingParser
{
    private const char QuoteChar = '"';
    private const char BracketStartChar = '[';
    private const char BracketEndChar = ']';
    private const char ParenthStartChar = '(';
    private const char ParenthEndChar = ')';
    private const char NewLineChar = '\n';
    private const string DialogStartTagName = "#(dialog)";
    private const string DialogEndTagName = "#end";
    private const string PhoneStartTagName = "#(phone)";
    private const string SMSStartTagName = "#(sms)";

    private const string QuoteStartTagName = "#(quote)";
    private const string QuoteEndTagName = DialogEndTagName;

    private static readonly Regex CommentStartRegex = new Regex(@"(\n?- *\(([a-z]| |[0-9]|-|,|&)+\))", RegexOptions.IgnoreCase);
    private static readonly Regex FileNameRegex = new Regex(@"([0-9]{4}-[0-9]{2}-[0-9]{2})([A-Z]+)?-(.*)\.story", RegexOptions.IgnoreCase);
    private static readonly Regex StoryTimeRegex = new Regex(@"#storystart\(([^\|]+)\|([^\)]+)\)", RegexOptions.IgnoreCase);
    private static readonly Regex SectionRegex = new Regex(@"#section\(([^\)]+)\)", RegexOptions.IgnoreCase);
    private static readonly Regex QuoteNameRegex = new Regex(@"#\(quote\)\[([^\|]*)(.*)\]", RegexOptions.IgnoreCase);

    private const string StoryTimeEndTagName = "#storyend";

    private static bool IsWhiteSpace(char ch) => ch == ' ' || ch == '\t';
    private static bool IsWhiteSpaceOrNewLine(char ch) => ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n';


    private static (string ChapterName, DateTime ChapterDate, string ChapterOrder) ParseFileName(string fileName)
    {
        var match = FileNameRegex.Match(fileName)!;
        var returnee = (
            ChapterName: match.Groups[3].Value,
            ChapterDate: DateTime.ParseExact(match.Groups[1].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            ChapterOrder: match.Groups[2].Value ?? ""
            );
        return returnee;
    }

    public static async Task<BookAboutTheAuthor> ParseAuthorInformation(string fileName)
    {
        var fileContent = await File.ReadAllTextAsync(fileName);
        var aliases = new Dictionary<string, string>();
        var characterInfos = new Dictionary<string, BookCharacterInfo>();
        var contents = ParseFileContents(fileContent, currentIndex: 0, metadata: null, aliases: aliases, characterInfos: characterInfos);
        var returnee = new BookAboutTheAuthor(contents);
        return returnee;
    }


    public static async Task<BookChapter> ParseChapter(string fileName, IReadOnlyDictionary<string, BookCharacterInfo> characterInfos)
    {
        var fileContent = await File.ReadAllTextAsync(fileName);
        var metadataParseResult = ParseMetaData(fileContent);
        var metadata = metadataParseResult?.Result;
        var aliases = metadata?.Aliases ?? new Dictionary<string, string>();
        var currentIndex = metadataParseResult?.CurrentIndex ?? 0;
        var chapterContent = ParseFileContents(fileContent: fileContent, currentIndex: currentIndex, metadata: metadata, aliases: aliases, characterInfos: characterInfos);
        var (title, chapterDate, chapterOrder) = ParseFileName(fileName);
        var returnee = new BookChapter(
            ChapterDate: chapterDate,
            ChapterName: title,
            ChapterContents: chapterContent,
            MetaData: metadata,
            ChapterOrder: chapterOrder
            );

        return returnee;
    }
    public static IReadOnlyCollection<BookChapterContent> ParseFileContents(
        string fileContent,
        IReadOnlyDictionary<string, string>? aliases = null,
        IReadOnlyDictionary<string, BookCharacterInfo>? characterInfos = null) =>
        ParseFileContents(
            fileContent,
            currentIndex: 0,
            metadata: null,
            aliases: aliases ?? new Dictionary<string, string>(),
            characterInfos: characterInfos ?? new Dictionary<string, BookCharacterInfo>()
           );


    private static IReadOnlyCollection<BookChapterContent> ParseFileContents(
        string fileContent, 
        int currentIndex, 
        BookChapterMetaData? metadata, 
        IReadOnlyDictionary<string, string> aliases,
        IReadOnlyDictionary<string, BookCharacterInfo> characterInfos
        )
    {
        var parsed = new List<TopLevelPart>();
        while (true)
        {
            var asContextBreak = TryParseContextBreak(fileContent, currentIndex);
            if (asContextBreak != null)
            {
                parsed.Add(asContextBreak.Result);
                currentIndex = asContextBreak.CurrentIndex;
                continue;
            }

            var asQuote = TryParseQuote(fileContent, currentIndex);
            if (asQuote != null)
            {
                parsed.Add(asQuote.Result);
                currentIndex = asQuote.CurrentIndex;
                continue;
            }

            var asDialog = TryParseDialog(fileContent, currentIndex);
            if (asDialog != null)
            {
                parsed.Add(asDialog.Result);
                currentIndex = asDialog.CurrentIndex;
                continue;
            }

            var asSection = TryParseSection(fileContent, currentIndex);
            if (asSection != null)
            {
                parsed.Add(asSection.Result);
                currentIndex = asSection.CurrentIndex;
                continue;
            }

            var asStoryTime = TryParseStoryTime(fileContent, currentIndex);
            if (asStoryTime != null)
            {
                parsed.Add(asStoryTime.Result);
                currentIndex = asStoryTime.CurrentIndex;
                continue;
            }

            var asComment = TryParseCharacterAction(fileContent, currentIndex);
            if (asComment != null)
            {
                parsed.Add(asComment.Result);
                currentIndex = asComment.CurrentIndex;
                continue;
            }
            var asNarrations = TryParseNarration(fileContent, currentIndex);
            if (asNarrations != null)
            {
                parsed.AddRange(asNarrations.Select(_ => _.Result));
                currentIndex = asNarrations.Max(_ => _.CurrentIndex);
                continue;
            }
            break;
        }
        var returnee = parsed
            .Select(_ => _.ToDomain(characterInfos, aliases))
            .ToList();
        return returnee;
    }


    private static ParseResult<BookChapterMetaData>? ParseMetaData(string input)
    {
        input = input.Trim();
        if (input.StartsWith("{") && input.Contains('}'))
        {
            var currentIndex = 1;
            var currentLevel = 1;
            while(currentLevel > 0 && currentIndex + 1 < input.Length)
            {
                if (input[currentIndex] == '{')
                    currentLevel++;
                else if (input[currentIndex] == '}')
                    currentLevel--;
                currentIndex += 1;
            }
            if (currentLevel > 0)
                return null;
            var jsonContent = input.Captured(0, currentIndex + 1);
            try
            {
                var parsed = JsonSerializer.Deserialize<BookChapterMetaData>(jsonContent)!;
                parsed = parsed with
                {
                    Aliases = parsed.Aliases?
                       .Select(_ => (CharacterKey: _.Key.AsNameKey(), Name: _.Value))?
                       .ToDictionarySafe(_ => _.CharacterKey, _ => _.Name.AsNameKey())
                };
                return new ParseResult<BookChapterMetaData>(input, parsed, CurrentIndex: currentIndex + 1, ParsedPart: jsonContent);
            }
            catch(Exception e) 
            {
                var tess = e;
            
            }
        }
        return null;

    }

    private static ParseResult<Section>? TryParseSection(string input, int startIndex)
    {
        var currentIndex = startIndex;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        if (currentIndex >= input.Length)
            return null;
        if (!input.Substring(currentIndex).StartsWith("#section"))
            return null;
        var match = SectionRegex.Matches(input, currentIndex).FirstOrDefault();
        if (match == null || match.Groups.Count < 2)
            return null;
        var title = match.Groups[1].Value;
        return new ParseResult<Section>(input, new Section(title), currentIndex + match.Length, input.Captured(startIndex, currentIndex + match.Length));

    }


    private static ParseResult<ContextBreak>? TryParseContextBreak(string input, int startIndex)
    {
        var currentIndex = startIndex;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        if (currentIndex + 3 > input.Length)
            return null;
        var lines = input.Substring(currentIndex).Split('\n');
        var firstLine = lines[0];
        firstLine = firstLine.Replace("\r", "").Trim();
        var firstLineContent = firstLine.Select(_ => _).ToHashSet();
        if (firstLineContent.Count == 1 && firstLineContent.Contains('.'))
            return new ParseResult<ContextBreak>(input, new ContextBreak(), currentIndex + lines[0].Length, lines[0]);
        else return null;

    }


    private static ParseResult<StoryTime>? TryParseStoryTime(string input, int startIndex)
    {
        var currentIndex = startIndex;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        if (currentIndex >= input.Length)
            return null;
        if (!input.Substring(currentIndex).StartsWith("#storystart", StringComparison.InvariantCultureIgnoreCase))
            return null;
        var startTagMatch = StoryTimeRegex.Matches(input, currentIndex).FirstOrDefault();
        var endTagMatchIndex = input.IndexOf(StoryTimeEndTagName, currentIndex, StringComparison.InvariantCultureIgnoreCase);
        if(startTagMatch == null || startTagMatch.Groups.Count < 3 || endTagMatchIndex < 0)
            return null;
        var (title, characterName) = (startTagMatch.Groups[1].Value.Trim(), startTagMatch.Groups[2].Value.Trim());
        var startTagEndIndex = currentIndex + startTagMatch.Length;
        var story = input.Captured(startTagEndIndex, endTagMatchIndex);
        var endTagEndIndex = endTagMatchIndex + StoryTimeEndTagName.Length;

        var returnee = new ParseResult<StoryTime>(
            input, 
            new StoryTime(characterName, title, story), 
            endTagEndIndex, 
            input.Captured(currentIndex, endTagEndIndex)
            );
        return returnee;
    }

    private static IReadOnlySet<string> StartTagsForStopping => AllDialogStartTags
        .ToHashSet();
    private static IReadOnlyCollection<ParseResult<TopLevelPart>>? TryParseNarration(string input, int startIndex)
    {
        var returnee = new List<ParseResult<TopLevelPart>>();
        var asComment = TryParseCharacterAction(input, startIndex);
        if (asComment != null)
            return null;
        var currentIndex = startIndex;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        if (currentIndex >= input.Length)
            return null;
        var narrationBuilder = new StringBuilder();
        var narrationListItems = new List<string>();
        var stopTags = StartTagsForStopping;
        while(currentIndex < input.Length)
        {
            var rest = currentIndex == 0 ? input : input.Substring(currentIndex - 1);
            if (currentIndex > 1 && rest.StartsWith("\n..."))
                break;
            var lowerRest = rest.ToLower().Trim();

            if (currentIndex > 1 && stopTags.Any(tag => lowerRest.StartsWith(tag)))
                break;
            if (!input.Substring(currentIndex).Contains(NewLineChar))
            {
                var onThisLine = input.Substring(currentIndex);
                narrationBuilder.Append(onThisLine);
                currentIndex = input.Length;
                break;
            }
            var nextNewLine = input.IndexOf(NewLineChar, currentIndex);
            var thisLine = input.Captured(currentIndex, nextNewLine);
            if (thisLine.Trim().StartsWith("-"))
            {
                narrationListItems.Add(thisLine.Skip(1)
                    .MakeString("")
                    .Replace("\r",""));
            }
            else
            {
                EmptyNarrationListItems(narrationListItems, narrationBuilder, returnee, input, currentIndex);
                narrationBuilder.Append(thisLine +"\n");
            }
            currentIndex = nextNewLine + 1;
            asComment = TryParseCharacterAction(input, currentIndex);
            var asSection = TryParseSection(input, currentIndex);

            if (asComment != null || asSection != null)
            {
                EmptyNarrationListItems(narrationListItems, narrationBuilder, returnee, input, currentIndex);
                break;
            }
            ForwardPastSpaces(ref currentIndex, input);
        }
        EmptyNarrationListItems(narrationListItems, narrationBuilder, returnee, input, currentIndex);
        if (narrationBuilder.Length == 0 && returnee.Count == 0)
            return null;
        if(narrationBuilder.Length > 0)
        {
            var narrationString = narrationBuilder.ToString();
            var narration = new Narration(narrationString);
            returnee.Add(new ParseResult<TopLevelPart>(input, narration, currentIndex, narrationString));
        }
        var startString = input.Substring(startIndex);
        return returnee;


    }

    private static void EmptyNarrationListItems(List<string> narrationListItems, StringBuilder narrationBuilder, List<ParseResult<TopLevelPart>> topLevelParts, string inputString, int currentIndex) 
    {
        if (narrationListItems.Count > 1)
        {
            if(narrationBuilder.Length > 0)
            {
                var narration = new Narration(narrationBuilder.ToString());
                topLevelParts.Add(new ParseResult<TopLevelPart>(inputString, narration, currentIndex, narration.Content));
                narrationBuilder.Clear();
            }
            var allCaptured = narrationListItems.MakeString("\n-");
            topLevelParts.Add(new ParseResult<TopLevelPart>(inputString, new NarrationList(narrationListItems.ToArray(), IsNumbered: false), currentIndex, allCaptured));
            narrationListItems.Clear();
        }
        else if (narrationListItems.Count == 1)
        {
            narrationBuilder.AppendLine(narrationListItems[0] + "\n");
            narrationListItems.Clear();
        }

    }
    private static ParseResult<TopLevelPart>? TryParseQuote(string input, int startIndex)
    {
        var currentIndex = startIndex;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        var rest = input.Substring(currentIndex);
        if (!rest.ToLower().StartsWith(QuoteStartTagName))
            return null;
        (string? name, string? subName) = (null, null);
        var match = QuoteNameRegex.Matches(rest)
            .FirstOrDefault();
        if(match != null)
        {
            if (match.Groups.Count > 2)
                (name, subName) = (match.Groups[1].Value, match.Groups[2].Value.Replace("|",""));
            else name = match.Groups[1].Value;
        }
        currentIndex = input.IndexOf("\n", currentIndex) + 1;
        var endTagIndex = input.IndexOf(QuoteEndTagName, startIndex);
        if (endTagIndex < 0)
            endTagIndex = input.Length - 1;
        var matched = input.Captured(currentIndex, endTagIndex);
        currentIndex = endTagIndex + QuoteEndTagName.Length + 1;
        if(currentIndex >= input.Length)
            currentIndex = input.Length - 1;
        var quote = new Quote(name, subName, matched);
        var returnee = new ParseResult<TopLevelPart>(input, quote, currentIndex, matched);
        return returnee;
        
    }

    private static readonly IReadOnlyCollection<string> AllDialogStartTags = 
        new List<string> { DialogStartTagName, PhoneStartTagName, SMSStartTagName }.ToHashSet();
    private static ParseResult<TopLevelPart>? TryParseDialog(string input, int startIndex)
    {
        var currentIndex = startIndex;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        var rest = input.Substring(currentIndex);
        var lowerRest = rest.ToLower();
        if (!AllDialogStartTags.Any(tag => lowerRest.StartsWith(tag)))
            return null;
        BookInteractionType? interactionType = null;
        if (lowerRest.StartsWith(PhoneStartTagName))
            interactionType = BookInteractionType.PhoneCall;
        if (lowerRest.StartsWith(SMSStartTagName))
            interactionType = BookInteractionType.SMS;
        var comments = new List<CharacterComment>();
        var doneYet = false;
        currentIndex = input.IndexOf("\n", currentIndex) - 1;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        while (!doneYet)
        {
            var characterCommentResult = TryParseCharacterAction(input, currentIndex);
            if(characterCommentResult != null && characterCommentResult.Result is CharacterComment comm)
            {
                comments.Add(comm);
                currentIndex = characterCommentResult.CurrentIndex;
            }
            else
            {
                currentIndex = input.IndexOf("\n", currentIndex) - 1;
            }
            ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
            rest = input.Substring(currentIndex);
            if(rest.ToLower().StartsWith(DialogEndTagName))
                doneYet = true;

        }
        currentIndex = input.ToLower().IndexOf(DialogEndTagName, startIndex) + DialogEndTagName.Length;
        var leftSide = comments
            .First().CharacterName;
        var rightSide = comments
            .Where(_ => _.CharacterName != leftSide)
            .First().CharacterName;
        var matchedString = input.Captured(startIndex, currentIndex);

        return new ParseResult<TopLevelPart>(
            Input: input,
            Result: new Dialog(leftSide, rightSide, comments, interactionType),
            CurrentIndex: currentIndex,
            ParsedPart: matchedString);
    }


    private static ParseResult<TopLevelPart>? TryParseCharacterAction(string input, int startIndex)
    {
        var currentIndex = startIndex;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        var nameMatch = CommentStartRegex.Matches(input, currentIndex).FirstOrDefault();
        if (nameMatch == null || nameMatch.Index != currentIndex)
            return null;
        var nameMatchValue = nameMatch.Value;
        nameMatchValue = nameMatchValue.Captured(nameMatchValue.IndexOf(ParenthStartChar) + 1, nameMatchValue.IndexOf(ParenthEndChar));
        var isThinking = false;
        var isSinging = false;
        var name = nameMatchValue.Trim();
        BookInteractionType? interactionType = null;
        if(nameMatchValue.Contains('-'))
        {
            var splitted = nameMatchValue.Split('-');
            name = splitted[0].Trim();
            var matchOn = splitted[1].ToLower();
            isThinking = matchOn.Contains("think");
            isSinging = matchOn.Contains("sing");
            if (matchOn.Contains("phone"))
                interactionType = BookInteractionType.PhoneCall;
            else if (matchOn.Contains("sms"))
                interactionType = BookInteractionType.SMS;

        }
        currentIndex = nameMatch.Index + nameMatch.Value.Length;
        if (currentIndex >= input.Length)
            return null;
        var lines = new List<QuotedCommentLine>();
        while(true)
        {
            var curString = input.Substring(currentIndex);
            var parsedLine = TryParseCommentLine(input, currentIndex);
            if (parsedLine == null)
                break;
            lines.Add(parsedLine.Result);
            currentIndex = parsedLine.CurrentIndex;
        }
        if (!lines.Any())
            return null;

        var matchedString = input.Captured(startIndex, currentIndex);
        if (isSinging)
        {
            var song = new Singing(CharacterName: name, Content: lines.Select(_ => _.Comment).ToList());
            return new ParseResult<TopLevelPart>(input, song, currentIndex, matchedString);
        }

        var comment = new CharacterComment(CharacterName: name, CommentLines: lines, IsThinking: isThinking, interactionType);
        return new ParseResult<TopLevelPart>(input, comment, currentIndex, matchedString);
    }

    private static ParseResult<QuotedCommentLine>? TryParseCommentLine(string input, int startIndex)
    {
        var currentIndex = startIndex;
        ForwardPastSpaces(ref currentIndex, input, alsoForwardPastNewLine: true);
        if(currentIndex >= input.Length) return null;
        var curChar = input[currentIndex];
        if (curChar == QuoteChar && currentIndex + 1 < input.Length)
            return ParseQuotedCommentLine(input, currentIndex + 1);
        return null;
    }


    private static ParseResult<QuotedCommentLine> ParseQuotedCommentLine(string input, int startIndex)
    {
        var commentLineResult = ParsePart(input, startIndex, QuoteChar);
        if (!commentLineResult.HasMore)
            return new ParseResult<QuotedCommentLine>(input, new QuotedCommentLine(Comment: commentLineResult.Result), commentLineResult.CurrentIndex, commentLineResult.ParsedPart);

        var currentIndex = commentLineResult.CurrentIndex;
        string? commentDescription = null;
        ForwardPastSpaces(ref currentIndex, input);
        if (input.Length > currentIndex + 1 && input[currentIndex] == BracketStartChar)
        {
            var descResult = ParsePart(input, currentIndex + 1, BracketEndChar);
            currentIndex = descResult.CurrentIndex;
            commentDescription = descResult.Result;
        }

        var returnee = new QuotedCommentLine(Comment: commentLineResult.Result, Description: commentDescription);
        var parsedPart = input.Captured(startIndex, currentIndex - 1);
        return new ParseResult<QuotedCommentLine>(input, returnee, currentIndex, parsedPart);

    }

    private static ParseResult<string> ParsePart(string input, int startIndex, char matchChar) =>
        ParsePart(input, startIndex, matchChar, _ => _);

    private static ParseResult<TRes> ParsePart<TRes>(string input, int startIndex, char matchChar, Func<string, TRes> mapper)
    {
        var relPart = input.Substring(startIndex);
        if (!relPart.Contains(matchChar))
        {
            var mapped = mapper(relPart);
            return new ParseResult<TRes>(input, mapped, input.Length, relPart);
        }
        var endIndex = input.IndexOf(matchChar, startIndex);
        var captured = input.Captured(startIndex, endIndex);
        var mappedRes = mapper(captured);
        if (input.Length < endIndex + 1)
            return new ParseResult<TRes>(input, mappedRes, endIndex, captured);
        return new ParseResult<TRes>(input, mappedRes, endIndex + 1, captured);

    }

    private static void ForwardPastSpaces(ref int currentIndex, string input, bool alsoForwardPastNewLine = false)
    {
        while(currentIndex < input.Length)
        {
            if(alsoForwardPastNewLine)
            {
                if (IsWhiteSpaceOrNewLine(input[currentIndex]))
                {
                    currentIndex += 1;
                    continue;
                }
            }
            else if(IsWhiteSpace(input[currentIndex]))
            {
                currentIndex += 1;
                continue;
            }
            break;
        }
        if (currentIndex > input.Length)
            currentIndex = input.Length;
    }

    private static string Captured(this string inp, int startIndex, int endIndex) => (startIndex, endIndex) switch
    {
        _ when startIndex >= endIndex => "",
        _ when endIndex >= inp.Length => inp.Substring(startIndex),
        _ => inp.Substring(startIndex, endIndex - startIndex)
    };

    private static BookCharacter CharacterFrom(string characterName, IReadOnlyDictionary<string, BookCharacterInfo> characters, IReadOnlyDictionary<string, string> aliases) =>
        new BookCharacter(characterName, CharacterInfo: null, Alias: aliases.GetValueOrDefault(characterName.AsNameKey())?.AsNameKey()).Pipe(ch => ch with
        {
            CharacterInfo = (ch.Alias ?? ch.CharacterKey).Pipe(cId => characters.GetValueOrDefault(cId))
        });

    private static IReadOnlyCollection<string> CleanSongLines(IEnumerable<string> lines)
    {
        var trimmed = lines.SelectMany(lin => lin.Split("\n"))
            .Select(line => line
                .Replace('\r', ' ')
                .Trim()
             ).ToList();
        trimmed.Add("");
        return trimmed;
    }

    private static string AsNameKey(this string characterName) => characterName
        .Trim()
        .ToLower()
        .Replace(" ", "");






    private abstract record TopLevelPart()
    {
        public BookChapterContent ToDomain(IReadOnlyDictionary<string, BookCharacterInfo> characters, IReadOnlyDictionary<string, string> aliases) => this switch
        {
            Narration narr => new BookNarration(narr.Content),
            CharacterComment comm => new BookCharacterLine(
                Character: CharacterFrom(comm.CharacterName, characters, aliases),
                LineParts: comm.CommentLines
                   .Select(lp => new BookCharacterLinePart(lp.Comment, lp.Description))
                   .ToList(),
                IsThought: comm.IsThinking,
                InteractionType: comm.InteractionType
                ),
            Dialog dia => new BookDialog(
                LeftSide: CharacterFrom(dia.CharacterLeft, characters, aliases),
                RightSide: CharacterFrom(dia.CharacterRight, characters, aliases),
                Entries: dia.Comments
                   .Select(comm => 
                              comm.CharacterName.ToLower().Trim() == dia.CharacterLeft.ToLower().Trim() ? 
                              BookDialogEntry.Left((comm.ToDomain(characters, aliases) as BookCharacterLine)!) :
                              BookDialogEntry.Right((comm.ToDomain(characters, aliases) as BookCharacterLine)!)
                   ).ToList(),
                InteractionType: dia.InteractionType
            ),
            ContextBreak ctb => new BookContextBreak(),
            Singing sing => new BookSinging(
                Character: CharacterFrom(sing.CharacterName, characters, aliases),
                LinesSong: CleanSongLines(sing.Content)

            ),
            NarrationList lis => new BookNarrationList(lis.Items, lis.IsNumbered),
            StoryTime st => new BookCharacterStoryTime(
                Character: CharacterFrom(st.CharacterName, characters, aliases), 
                Title: st.Title, 
                Story: st.Story
                ),
            Section sec => new BookChapterSection(sec.Title),
            Quote quot => new BookQuote(quot.Name, quot.SubName, quot.QuoteText),
            _ => throw new Exception("WHAT???")
        };
    }

    private record Section(string Title) : TopLevelPart;
    private record Narration(string Content) : TopLevelPart;
    private record NarrationList(IReadOnlyCollection<string> Items, bool IsNumbered) : TopLevelPart;
    private record Singing(string CharacterName, IReadOnlyCollection<string> Content) : TopLevelPart;
    private record StoryTime(string CharacterName, string Title, string Story) : TopLevelPart;
    private record ContextBreak() : TopLevelPart();
    private record CharacterComment(string CharacterName, IReadOnlyCollection<QuotedCommentLine> CommentLines, bool IsThinking = false, BookInteractionType? InteractionType = null) : TopLevelPart;
    private record QuotedCommentLine(string Comment, string? Description = null);
    private record Dialog(string CharacterLeft, string CharacterRight, IReadOnlyCollection<CharacterComment> Comments, BookInteractionType? InteractionType) : TopLevelPart;

    private record Quote(string? Name, string? SubName, string QuoteText) : TopLevelPart;

}
