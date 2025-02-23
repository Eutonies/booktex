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


