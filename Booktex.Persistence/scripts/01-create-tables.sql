create type subscription_type as 
enum('GitHub');


create table subscription (
    subscription_id bigint primary key generated always as identity,
    subscription_type subscription_type not null,
    github_owner varchar(200),
    github_repo varchar(200),
    file_regex varchar(2000)
);

create table subscription_execution(
   execution_id bigint primary key generated always as identity,
   subscription_id bigint not null,
   execution_time timestamp not null,
   hash_code varchar(1000) not null,
   unique(subscription_id, hash_code),
   constraint fk_subscription_exec_sub foreign key(subscription_id) references subscription(subscription_id) on delete cascade
);

create index idx_subscription_execution_subid on subscription_execution(subscription_id);

create table subscription_execution_file(
   exec_file_id bigint primary key generated always as identity,
   execution_id bigint not null,
   absolute_file_name varchar(2000) not null,
   file_content bytea not null,
   constraint fk_subscription_exec_file_exec foreign key(execution_id) references subscription_execution(execution_id) on delete cascade
);

create index idx_subscription_execution_fil_execid on subscription_execution_file(execution_id);


##### Book, chapter and sections


create table book_release(
   release_id bigint primary key generated always as identity,
   release_author varchar(100) not null,
   release_version varchar(20) not null,
   last_modified timestamp not null
);


create table book_story_line(
   story_line_id bigint primary key generated always as identity,
   release_id bigint not null,
   story_line varchar(200) not null,
   story_line_name varchar(300) not null,
   story_line_image varchar(300) not null,
   story_line_video varchar(300) not null,
   story_line_font varchar(300) not null,
   story_line_att_text varchar(2000) not null,
   story_line_att_link varchar(2000) not null,
   constraint fk_book_story_line_relid foreign key(release_id) references book_release(release_id) on delete cascade
);

create index idx_book_story_line_relid on book_story_line(release_id);

create table book_chapter(
  chapter_id bigint primary key generated always as identity,
  release_id bigint not null,
  chapter_date date not null,
  chapter_name varchar(300) not null,
  chapter_order varchar(100) not null,
  constraint fk_book_chapter_relid foreign key(release_id) references book_release(release_id) on delete cascade
);

create index idx_book_chapter_relid on book_chapter(release_id);

create table book_chapter_metadata(
  metadata_id bigint primary key generated always as identity,
  chapter_id bigint not null,
  chapter_title varchar(1000),
  chapter_date date,
  story_line_key varchar(200),
  chapter_order varchar(100),
  constraint fk_book_chapter_metadata_chapid foreign key(chapter_id) references book_chapter(chapter_id) on delete cascade
);

create index idx_book_chapter_metadata_chapid on book_chapter_metadata(chapter_id);


create table book_chapter_metadata_mapping(
  metadata_mapping_id bigint primary key generated always as identity,
  metadata_id bigint not null,
  map_from varchar(200) not null,
  map_to varchar(200) not null,
  constraint fk_book_chapter_metadata_mapping_metid foreign key(metadata_id) references book_chapter_metadata(metadata_id) on delete cascade
);
create index idx_book_chapter_metatadata_mapping_metid on book_chapter_metadata(metadata_id);


create table book_character(
  character_id bigint primary key generated always as identity,
  release_id bigint,
  chapter_id bigint,
  character_name varchar(1000) not null,
  chapter_date date,
  info_character varchar(200),
  info_showname varchar(1000),
  info_color varchar(20),
  info_font varchar(200),
  info_sitefont varchar(1000),
  character_alias varchar(200)
);

create index idx_book_character_refs on book_character(release_id,chapter_id);


create type content_type as 
enum('ChapterSection', 'CharacterLine', 'CharacterStoryTime', 'ContextBreak', 'Narration', 'NarrationList', 'BookSinging');

create table book_chapter_content(
  content_id bigint primary key generated always as identity,
  about_the_author_id bigint,
  chapter_id bigint,
  content_index integer,
  content_type content_type not null,
  title_data varchar(300),
  string_data text,
  is_though boolean,
  is_numbered boolean,
  character_id bigint
);

create index idx_book_chapter_content_refs on book_chapter_content(about_the_author_id,chapter_id);

create table book_chapter_content_sub(
  content_sub_id primary key generated always as identity,
  content_id bigint not null,
  string_data text,
  string_data_description varchar(1000),
  content_sub_index integer not null,
  constraint fk_book_chapter_content_contid foreign key(content_id) references book_chapter_content(content_id) on delete cascade
);
create index idx_book_chapter_content_sub_contid on book_chapter_content_sub(content_id);



