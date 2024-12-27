CREATE TABLE [dbo].[Users] (
                               [UserId]       INT            IDENTITY (1, 1) NOT NULL,
                               [FullName]     NVARCHAR (100) NOT NULL,
                               [EmailAddress] NVARCHAR (255) DEFAULT (N'') NOT NULL,
                               [Password]     NVARCHAR (255) NOT NULL,
                               PRIMARY KEY CLUSTERED ([UserId] ASC),
                               UNIQUE NONCLUSTERED ([EmailAddress] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_FullName]
    ON [dbo].[Users]([FullName] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_EmailAddress]
    ON [dbo].[Users]([EmailAddress] ASC);

CREATE TABLE [dbo].[Services] (
                                  [ServiceId]   INT           NOT NULL,
                                  [ServiceName] NVARCHAR (50) NOT NULL,
                                  PRIMARY KEY CLUSTERED ([ServiceId] ASC)
);


INSERT INTO Users (FullName, EmailAddress, Password)
VALUES
    ('John Smith', 'john.smith@guardian.com', 'hashed_password_1'),
    ('Sarah Johnson', 'sarah.j@guardian.com', 'hashed_password_2'),
    ('Michael Brown', 'michael.b@guardian.com', 'hashed_password_3'),
    ('Emma Wilson', 'emma.w@guardian.com', 'hashed_password_4'),
    ('David Lee', 'david.lee@guardian.com', 'hashed_password_5'),
    ('Lisa Anderson', 'lisa.a@guardian.com', 'hashed_password_6'),
    ('James Taylor', 'james.t@guardian.com', 'hashed_password_7'),
    ('Maria Garcia', 'maria.g@guardian.com', 'hashed_password_8');

INSERT INTO Services (ServiceId, ServiceName) VALUES
                                                  (21, 'Ftp'),
                                                  (22, 'Ssh'),
                                                  (23, 'Telnet'),
                                                  (25, 'Smtp'),
                                                  (53, 'Dns'),
                                                  (67, 'Dhcp'),
                                                  (69, 'Tftp'),
                                                  (80, 'Http'),
                                                  (110, 'Pop3'),
                                                  (123, 'Ntp'),
                                                  (143, 'Imap'),
                                                  (161, 'Snmp'),
                                                  (389, 'Ldap'),
                                                  (443, 'Https'),
                                                  (445, 'Smb'),
                                                  (3306, 'MySql'),
                                                  (3389, 'Rdp'),
                                                  (5432, 'PostgreSql'),
                                                  (6379, 'Redis'),
                                                  (27017, 'MongoDB');
CREATE TABLE [dbo].[Incidents] (
                                   [IncidentId]    INT            IDENTITY (1, 1) NOT NULL,
                                   [PacketEntryId] INT            NOT NULL,
                                   [Status]        NVARCHAR (50)  NOT NULL DEFAULT ('Open'),
                                   [AssignedTo]    NVARCHAR (100) NOT NULL,
                                   [Comment]       NVARCHAR (MAX) NULL,
                                   [ResolvedAt]    DATETIME       NULL,
                                   CONSTRAINT [PK_Incidents] PRIMARY KEY CLUSTERED ([IncidentId], [PacketEntryId]),
                                   CONSTRAINT [FK_Incidents_PacketEntries] FOREIGN KEY ([PacketEntryId]) REFERENCES [dbo].[PacketEntries] ([Id])
);

INSERT INTO [dbo].[PacketEntries]
([Src], [Dst], [Port], [ServiceId], [Info], [Attack], [ThreatLevel], [Timestamp])
VALUES
    ('192.168.1.100', '10.0.0.55', 3389, 3389, 'Multiple failed login attempts detected', 'Brute Force', 'High', '2024-12-08 09:15:23'),

    ('45.123.45.178', '192.168.1.25', 443, 443, 'SSL/TLS handshake failure', 'SSL Strip', 'Medium', '2024-12-08 10:30:45'),

    ('172.16.0.100', '192.168.1.10', 53, 53, 'DNS query amplification attempt', 'DNS Amplification', 'High', '2024-12-07 15:20:10'),

    ('10.0.0.15', '192.168.1.5', 80, 80, 'SQL injection attempt in HTTP GET request', 'SQL Injection', 'Critical', '2024-12-07 18:45:30'),

    ('192.168.1.50', '10.0.0.100', 22, 22, 'Successful SSH connection after multiple attempts', 'Suspicious Login', 'Medium', '2024-12-06 22:10:15'),

    ('45.89.123.45', '192.168.1.15', 3306, 3306, 'Database connection overflow attempt', 'Buffer Overflow', 'High', '2024-12-06 13:25:40'),

    ('192.168.1.75', '10.0.0.25', 445, 445, 'SMB protocol version scanning', 'Reconnaissance', 'Low', '2024-12-05 11:05:55'),

    ('10.0.0.30', '192.168.1.20', 25, 25, 'SMTP relay attempt detected', 'Email Spam', 'Medium', '2024-12-05 08:30:20'),

    ('172.16.0.50', '192.168.1.30', 161, 161, 'SNMP community string bruteforce', 'Brute Force', 'High', '2024-12-04 16:50:35'),

    ('192.168.1.60', '10.0.0.40', 27017, 27017, 'NoSQL injection attempt', 'Injection Attack', 'Critical', '2024-12-04 14:15:25');


INSERT INTO [dbo].[Incidents] ([PacketEntryId], [Status], [AssignedTo], [Comment], [ResolvedAt])
VALUES
    (13, 'Open', 'Alice Smith', 'Investigating multiple failed RDP login attempts. Potential breach attempt.', NULL),

    (14, 'Resolved', 'John Doe', 'SSL Strip attack blocked. Updated security certificates and protocols.', '2024-12-08 12:30:45'),

    (15, 'Escalated', 'Bob Johnson', 'DNS amplification attack detected. Requires network team review.', NULL),

    (16, 'Resolved', 'Eve Adams', 'SQL injection attempt blocked. IP added to blocklist.', '2024-12-07 20:45:30'),

    (17, 'Open', 'Alice Smith', 'Investigating suspicious SSH login pattern.', NULL),

    (18, 'In Progress', 'Charlie Wilson', 'Database overflow attempt detected. Implementing additional security measures.', NULL),

    (19, 'Resolved', 'Diana Brown', 'SMB scanning detected. Enhanced monitoring implemented.', '2024-12-06 09:15:20'),

    (20, 'Open', 'Frank Miller', 'Investigating SMTP relay attempt. Checking mail server logs.', NULL);

CREATE VIEW [dbo].[IncidentDetailsView] AS
SELECT
    i.IncidentId,
    i.Status,
    i.AssignedTo,
    i.Comment,
    i.ResolvedAt,
    p.Src AS SourceIP,
    p.Dst AS DestinationIP,
    p.Port,
    s.ServiceName,
    p.Attack,
    p.ThreatLevel,
    p.Timestamp AS PacketTimestamp,
    u.FullName AS AssignedUser,
    u.EmailAddress AS AssignedUserEmail
FROM
    [dbo].[Incidents] i
        INNER JOIN
    [dbo].[PacketEntries] p ON i.PacketEntryId = p.Id
        INNER JOIN
    [dbo].[Services] s ON p.ServiceId = s.ServiceId
        INNER JOIN
    [dbo].[Users] u ON i.AssignedTo = u.FullName;