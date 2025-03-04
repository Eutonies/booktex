create type chapter_content_type as 
enum('ChapterSection','CharacterLine','CharacterStoryTime','ContextBreak','Narration','NarrationList','BookSinging');

create table book_release (
  release_id bigint primary key generated always as identity,
  subscription_id bigint not null,
  release_author varchar(200) not null,
  release_version varchar(50) not null,
  last_modified timestamp not null,
  constraint fk_book_release_subid foreign key(subscription_id) references subscription(subscription_id) on delete cascade
);

create index idx_book_release on book_release(subscription_id);


create table book_story_line (
  story_line_id bigint primary key generated always as identity,
  release_id bigint not null,
  story_line varchar(200) not null,
  story_line_name varchar(600) not null,
  story_line_image varchar(1000) not null,
  story_line_video varchar(1000) not null,
  story_line_font varchar(200) not null,
  story_line_att_text varchar(1000) not null,
  story_line_att_link varchar(1000) not null
);


create table book_chapter (
  chapter_id bigint primary key generated always as identity,
  release_id bigint not null,
  chapter_date date not null,
  chapter_name varchar(200) not null,
  chapter_order varchar(100) not null,
  constraint fk_book_chapter foreign key (release_id) references book_release(release_id) on delete cascade
);

create index idx_book_chapter_relid on book_chapter(release_id);

create table book_chapter_metadata(
  metadata_id bigint primary key generated always as identity,
  chapter_id bigint not null,
  chapter_title varchar(200) not null,
  chapter_date date not null,
  story_line_key varchar(100),
  chapter_order varchar(20),
  constraint fk_book_chapter_metadata_chapid foreign key (chapter_id) references book_chapter(chapter_id) on delete cascade
);

create index idx_book_chapter_metadata_chapid on book_chapter_metadata(chapter_id);

create table book_chapter_metadata_mapping(
  metadata_mapping_id bigint primary key generated always as identity,
  metadata_id bigint not null,
  map_from varchar(200) not null,
  map_to varchar(200) not null,
  constraint fk_book_chapter_metadata_mapping_metid foreign key (metadata_id) references book_chapter_metadata(metadata_id) on delete cascade
);

create index idx_book_chapter_metadata_mapping on book_chapter_metadata_mapping(metadata_id);

create table book_character (
  character_id bigint primary key generated always as identity,
  release_id bigint,
  chapter_id bigint,
  character_name varchar(200) not null,
  info_character varchar(200),
  info_showname varchar(200),
  info_color varchar(200),
  info_site_font varchar(200),
  character_alias varchar(200)
);

create table book_chapter_content(
  content_id bigint primary key generated always as identity,
  about_the_author_id bigint,
  chapter_id bigint,
  content_index int,
  content_type chapter_content_type not null,
  title_data varchar(200),
  string_data varchar(2000),
  is_thought boolean,
  is_numbered boolean,
  character_id bigint
);

create table book_chapter_content_sub(
  content_sub_id bigint primary key generated always as identity,
  content_id bigint not null,
  string_data varchar(2000),
  string_data_description varchar(2000),
  content_sub_index int not null,
  constraint fk_book_chapter_content_sub_contid foreign key (content_id) references book_chapter_content(content_id) on delete cascade
);

create index idx_book_chapter_content_sub on book_chapter_content_sub(content_id);

create table book_about_author(
  about_the_author_id bigint primary key generated always as identity,
  release_id bigint not null,
  constraint fk_book_about_auther foreign key (release_id) references book_release(release_id) on delete cascade
);

create index idx_book_about_author_relid on book_about_author(release_id);
