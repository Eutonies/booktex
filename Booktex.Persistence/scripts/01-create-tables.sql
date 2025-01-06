create type subscription_type as 
enum('GitHub');


create table subscription (
    subscription_id bigint primary key generated always as identity,
    subscription_type subscription_type not null,
    github_owner varchar(200),
    github_repo varchar(200)
);

