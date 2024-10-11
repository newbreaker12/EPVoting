CREATE TABLE VotingGroups (
    Id BIGSERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    ReadableId VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    Disabled BOOLEAN NOT NULL
);

CREATE TABLE VotingRoles (
    Id BIGSERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description VARCHAR(255)
);

CREATE TABLE VotingUsers (
    Id BIGSERIAL PRIMARY KEY,
    Email TEXT NOT NULL,
    FirstName VARCHAR(255),
    LastName VARCHAR(255),
    Password VARCHAR(2048),
    pincode VARCHAR(10),
    PhoneNumber VARCHAR(255),
    IsMEP BOOLEAN NOT NULL,
    RoleId BIGINT REFERENCES VotingRoles(Id),
    GroupId BIGINT REFERENCES VotingGroups(Id),
    AccessToken VARCHAR(2048),
    TokenCreated TIMESTAMP,
    TokenExpires TIMESTAMP,
    Disabled BOOLEAN NOT NULL
);

CREATE TABLE VotingUsersToken (
    Id BIGSERIAL PRIMARY KEY,
    Email TEXT NOT NULL,
    ExpirationDate TIMESTAMP NOT NULL
);

CREATE TABLE VotingArticle (
    Id BIGSERIAL PRIMARY KEY,
    GroupsId BIGINT REFERENCES VotingGroups(Id),
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    CreatedAt TIMESTAMP NOT NULL
);

CREATE TABLE VotingSubArticle (
    Id BIGSERIAL PRIMARY KEY,
    ArticleId BIGINT REFERENCES VotingArticle(Id),
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    CreatedAt TIMESTAMP NOT NULL
);

CREATE TABLE VotingSession (
    Id BIGSERIAL PRIMARY KEY,
    ArticleId BIGINT REFERENCES VotingArticle(Id),
    Name VARCHAR(255) NOT NULL,
    Description VARCHAR(255),
    FromDate TIMESTAMP NOT NULL,
    ToDate TIMESTAMP NOT NULL
);

CREATE TABLE VoteSubmit (
    Id BIGSERIAL PRIMARY KEY,
    UserEmail VARCHAR(255) NOT NULL,
    ArticleId BIGINT REFERENCES VotingArticle(Id)
);

CREATE TABLE Vote (
    Id BIGSERIAL PRIMARY KEY,
    UserEmail VARCHAR(255) NOT NULL,
    SubArticleId BIGINT REFERENCES VotingSubArticle(Id),
    Type INT NOT NULL
);

CREATE VIEW VoteStatistics AS
SELECT 
    VA.Name AS ArticleName,
    VSA.Name AS SubArticleName,
    VG.Name AS GroupName,
    COUNT(CASE WHEN V.Type = 2 THEN 1 END) AS InFavorCount,
    COUNT(CASE WHEN V.Type = 0 THEN 1 END) AS NotInFavorCount,
    COUNT(CASE WHEN V.Type = 1 THEN 1 END) AS NeutralCount
FROM 
    VotingArticle VA
JOIN VotingSubArticle VSA ON VA.Id = VSA.ArticleId
JOIN VotingGroups VG ON VA.GroupsId = VG.Id
JOIN Vote V ON VSA.Id = V.SubArticleId
GROUP BY VA.Name, VSA.Name, VG.Name;

CREATE VIEW VoteUserStatistics AS
SELECT 
    VA.Name AS ArticleName,
    VG.Name AS GroupName,
    COUNT(VS.id) AS UsersVoted
FROM 
    VotingArticle VA
JOIN VotingGroups VG ON VA.GroupsId = VG.Id
left JOIN votesubmit VS ON VS.articleid = VA.id 
GROUP BY VA.Id, VA.Name, VG.Name;


