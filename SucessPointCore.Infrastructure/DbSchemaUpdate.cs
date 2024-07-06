using Dapper;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using SucessPointCore.Domain.Constants;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Infrastructure.Interfaces;
using System.Data;
using System.Text;

namespace SucessPointCore.Infrastructure
{
    public class DbSchemaUpdate
    {
        readonly IErrorLogRepository ErrorLogRepository;
        private string DbConnectionString { get; set; }

        public DbSchemaUpdate(string dbConnectionString, IErrorLogRepository errorLogRepository)
        {
            DbConnectionString = dbConnectionString;
            ErrorLogRepository = errorLogRepository;
        }

        #region Tables

        /// <summary>
        /// It creates ErrorLog table which will store all the information related to erros.
        /// </summary>
        public async void CreateTable_sp_errorlog()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_errorlog` (");
                queryBuilder.AppendLine("`ErrorLogID` INT(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("`ErrorMessage` TEXT DEFAULT NULL,");
                queryBuilder.AppendLine("`StackTrace` TEXT DEFAULT NULL,");
                queryBuilder.AppendLine("`UserID` INT(11) DEFAULT NULL,");
                queryBuilder.AppendLine("`LogDate` DATETIME NOT NULL,");
                queryBuilder.AppendLine("PRIMARY KEY (`ErrorLogID`)");
                queryBuilder.AppendLine(") ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }

            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"table : CreateTable_sp_errorlog \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// It creates Users table which will storing user(student) of platform including admin
        /// </summary>
        public async void CreateTable_Sp_User()
        {
            try
            {


                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_user` (");
                queryBuilder.AppendLine("`UserID` INT(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("`UserName` VARCHAR(100) NOT NULL,");
                queryBuilder.AppendLine("`Password` VARCHAR(250) NOT NULL,");
                queryBuilder.AppendLine("`UserType` TINYINT(1) NOT NULL,");
                queryBuilder.AppendLine("`Active` TINYINT(1) NOT NULL DEFAULT 0,");
                queryBuilder.AppendLine("`PasswordKey` VARCHAR(10) DEFAULT NULL,");
                queryBuilder.AppendLine("`CreatedOn` DateTime NOT NULL,");
                queryBuilder.AppendLine("`DisplayName` VARCHAR(50) NULL,");
                queryBuilder.AppendLine("PRIMARY KEY (`UserID`)");
                queryBuilder.AppendLine(") ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }

            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"table : CreateTable_Sp_User \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// It creates Standard table which will store student class/standard information
        /// </summary>
        public async void CreateTable_SP_Standard()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_standard` (");
                queryBuilder.AppendLine("`StandardID` INT(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("`StandardName` VARCHAR(50) NOT NULL,");
                queryBuilder.AppendLine("`Active` BIT(1) NOT NULL DEFAULT b'1',");
                queryBuilder.AppendLine("PRIMARY KEY (`StandardID`)");
                queryBuilder.AppendLine(") ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"table : CreateTable_SP_Standard \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// It creates Course table which will store course that student can enroll if needed.
        /// </summary>
        public async void CreateTable_sp_course()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_course` (");
                queryBuilder.AppendLine("  `CourseID` int(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("  `CourseName` varchar(200) NOT NULL,");
                queryBuilder.AppendLine("  `IsActive` tinyint(1) NOT NULL,");
                queryBuilder.AppendLine("  `CreatedBy` int(11) NOT NULL,");
                queryBuilder.AppendLine("  `CreatedOn` datetime NOT NULL,");
                queryBuilder.AppendLine("  `ChangedBy` int(11) DEFAULT NULL,");
                queryBuilder.AppendLine("  `ChangedOn` datetime DEFAULT NULL,");
                queryBuilder.AppendLine("  PRIMARY KEY (`CourseID`),");
                queryBuilder.AppendLine("  KEY `UserID` (`CreatedBy`),");
                queryBuilder.AppendLine("  KEY `fk_sp_course_changedby` (`ChangedBy`),");
                queryBuilder.AppendLine("  CONSTRAINT `fk_sp_course_changedby` FOREIGN KEY (`ChangedBy`) REFERENCES `sp_user` (`UserID`),");
                queryBuilder.AppendLine("  CONSTRAINT `fk_sp_course_createdby` FOREIGN KEY (`CreatedBy`) REFERENCES `sp_user` (`UserID`)");
                queryBuilder.AppendLine(") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");


                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_course \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// It creates coursevideos table that store information of videos of various courses
        /// </summary>
        public async void CreateTable_sp_coursevideos()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_coursevideos` (");
                queryBuilder.AppendLine("`VideoID` INT(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("`VideoName` VARCHAR(50) NOT NULL,");
                queryBuilder.AppendLine("`CourseID` INT(11) NOT NULL,");
                queryBuilder.AppendLine("`Order` INT(11) NOT NULL,");
                queryBuilder.AppendLine("`VideoHeading` VARCHAR(100) NOT NULL,");
                queryBuilder.AppendLine("PRIMARY KEY (`VideoID`)");
                queryBuilder.AppendLine(") ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_coursevideos \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());

            }
        }

        public async void CreateTable_sp_student()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_student` (");
                queryBuilder.AppendLine("  `StudentID` int(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("  `UserID` int(11) NOT NULL,");
                queryBuilder.AppendLine("  `DisplayName` varchar(50) NOT NULL,");
                queryBuilder.AppendLine("  `StandardID` int(11) NOT NULL,");
                queryBuilder.AppendLine("  `CourseID` int(11) DEFAULT NULL,");
                queryBuilder.AppendLine("  PRIMARY KEY (`StudentID`),");
                queryBuilder.AppendLine("  KEY `fk_sp_student_user_userid` (`UserID`),");
                queryBuilder.AppendLine("  KEY `fk_sp_student_course_courseid` (`CourseID`),");
                queryBuilder.AppendLine("  KEY `fk_sp_student_standard_standardid` (`StandardID`),");
                queryBuilder.AppendLine("  CONSTRAINT `fk_sp_student_course_courseid` FOREIGN KEY (`CourseID`) REFERENCES `sp_course` (`CourseID`) ON DELETE NO ACTION,");
                queryBuilder.AppendLine("  CONSTRAINT `fk_sp_student_standard_standardid` FOREIGN KEY (`StandardID`) REFERENCES `sp_standard` (`StandardID`),");
                queryBuilder.AppendLine("  CONSTRAINT `fk_sp_student_user_userid` FOREIGN KEY (`UserID`) REFERENCES `sp_user` (`UserID`)");
                queryBuilder.AppendLine(") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");



                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"table : CreateTable_sp_student \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void CreateTable_sp_studentCourse()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_studentcourse` (");
                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_studentcourse` (");
                queryBuilder.AppendLine("  `StudentCourseID` int(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("  `UserID` int(11) NOT NULL,");
                queryBuilder.AppendLine("  `StandardID` int(11) NOT NULL,");
                queryBuilder.AppendLine("  `CourseID` int(11) DEFAULT NULL,");
                queryBuilder.AppendLine("  PRIMARY KEY (`StudentCourseID`),");
                queryBuilder.AppendLine("  KEY `fk_sp_student_user_userid` (`UserID`),");
                queryBuilder.AppendLine("  KEY `fk_sp_student_course_courseid` (`CourseID`),");
                queryBuilder.AppendLine("  KEY `fk_sp_student_standard_standardid` (`StandardID`),");
                queryBuilder.AppendLine("  CONSTRAINT `fk_sp_student_course_courseid` FOREIGN KEY (`CourseID`) REFERENCES `sp_course` (`CourseID`) ON DELETE NO ACTION,");
                queryBuilder.AppendLine("  CONSTRAINT `fk_sp_student_standard_standardid` FOREIGN KEY (`StandardID`) REFERENCES `sp_standard` (`StandardID`),");
                queryBuilder.AppendLine("  CONSTRAINT `fk_sp_student_user_userid` FOREIGN KEY (`UserID`) REFERENCES `sp_user` (`UserID`)");
                queryBuilder.AppendLine(") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_studentCourse \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void CreateTable_sp_studentActivity()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_student_activity` (");
                queryBuilder.AppendLine("  `ActivityID` bigint(20) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("  `Description` varchar(100) NOT NULL,");
                queryBuilder.AppendLine("  `UserID` int(11) NOT NULL,");
                queryBuilder.AppendLine("  PRIMARY KEY (`ActivityID`)");
                queryBuilder.AppendLine(") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_studentActivity \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }


        //It creates FileInfo table which stores information of videos uploaded on system by admin
        public async void CreteTable_sp_filesinfo()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_filesinfo` (");
                queryBuilder.AppendLine("`FilesID` INT(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("`FileName` VARCHAR(500) NOT NULL,");
                queryBuilder.AppendLine("`FileType` VARCHAR(50) NOT NULL,");
                queryBuilder.AppendLine("`CourseID` INT(11) NOT NULL,");
                queryBuilder.AppendLine("`OrderNo` INT(11) NOT NULL,");
                queryBuilder.AppendLine("PRIMARY KEY (`FilesID`),");
                queryBuilder.AppendLine("KEY `CourseID` (`CourseID`),");
                queryBuilder.AppendLine("CONSTRAINT `SP_FilesInfo_ibfk_1` FOREIGN KEY (`CourseID`) REFERENCES `sp_course` (`CourseID`)");
                queryBuilder.AppendLine(") ENGINE=INNODB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreteTable_sp_filesinfo \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void CreteTable_sp_refreshtoken()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_refreshtoken` (");
                queryBuilder.AppendLine("  `RefreshTokenID` int(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("  `RefreshToken` char(36) NOT NULL,");
                queryBuilder.AppendLine("  `UserID` int(11) NOT NULL,");
                queryBuilder.AppendLine("  `CreatedOn` datetime NOT NULL,");
                queryBuilder.AppendLine("  `UpdatedOn` datetime DEFAULT NULL,");
                queryBuilder.AppendLine("  `ExpiresAt` datetime NOT NULL,");
                queryBuilder.AppendLine("  PRIMARY KEY (`RefreshTokenID`),");
                queryBuilder.AppendLine("  KEY `fk_refreshtoken_user_userid` (`UserID`),");
                queryBuilder.AppendLine("  CONSTRAINT `fk_refreshtoken_user_userid` FOREIGN KEY (`UserID`) REFERENCES `sp_user` (`UserID`)");
                queryBuilder.AppendLine(") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreteTable_sp_refreshtoken \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void CreteTable_sp_verification()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `verification` (");
                queryBuilder.AppendLine("  `VID` char(36) NOT NULL,");
                queryBuilder.AppendLine("  `UserID` int(11) NOT NULL,");
                queryBuilder.AppendLine("  `CreatedOn` datetime NOT NULL,");
                queryBuilder.AppendLine("  `UpdatedOn` datetime DEFAULT NULL,");
                queryBuilder.AppendLine("  `ExpiryTime` datetime NOT NULL,");
                queryBuilder.AppendLine("  `Type` smallint(6) NOT NULL,");
                queryBuilder.AppendLine("  `TFC` int(6) DEFAULT NULL,");
                queryBuilder.AppendLine("  KEY `IX_Verification_VID_UserID_ExpiryTime` (`VID`,`UserID`,`ExpiryTime`)");
                queryBuilder.AppendLine(") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreteTable_sp_verification \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region Procedures


        public async void Add_SP_ErrorLog_Insert()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_ErrorLog_Insert`;");

                // Create procedure statement
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_ErrorLog_Insert`(");
                queryBuilder.AppendLine("    IN p_ErrorMessage TEXT,");
                queryBuilder.AppendLine("    IN p_StackTrace TEXT,");
                queryBuilder.AppendLine("    IN p_UserID INT");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    INSERT INTO `sp_errorlog` (`ErrorMessage`, `StackTrace`, `UserID`, `LogDate`)");
                queryBuilder.AppendLine("    VALUES (p_ErrorMessage, p_StackTrace, p_UserID, NOW());");
                queryBuilder.AppendLine("END;");


                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_SP_ErrorLog_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_RefreshToken_Upsert()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure statement
                queryBuilder.AppendLine("/* Procedure structure for procedure `sp_SP_RefreshToken_Upsert` */");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_RefreshToken_Upsert`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_RefreshToken_Upsert`(");
                queryBuilder.AppendLine("    IN p_UserID INT,");
                queryBuilder.AppendLine("    IN p_RefreshToken VARCHAR(36),");
                queryBuilder.AppendLine("    IN p_Token_Expiry_Minute DATETIME");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    INSERT INTO `sp_refreshtoken` (`RefreshToken`, `UserID`, `CreatedOn`, `UpdatedOn`, `ExpiresAt`)");
                queryBuilder.AppendLine("    VALUES (p_RefreshToken, p_UserID, NOW(), NULL, p_Token_Expiry_Minute);");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }

            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_RefreshToken_Upsert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_User_CheckCredentials()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure statement
                queryBuilder.AppendLine("/* Procedure structure for procedure `sp_SP_User_CheckCredentials` */");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_User_CheckCredentials`;");

                // Create procedure statement
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_User_CheckCredentials`(");
                queryBuilder.AppendLine("    IN p_Username VARCHAR(100),");
                queryBuilder.AppendLine("    IN p_EncryptedPassword VARCHAR(250)");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    SELECT `UserID`, `UserType`, `Active`");
                queryBuilder.AppendLine("    FROM `sp_user`");
                queryBuilder.AppendLine("    WHERE `UserName` = p_Username AND `PASSWORD` = p_EncryptedPassword");
                queryBuilder.AppendLine("    LIMIT 1;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }

            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_CheckCredentials \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_User_GetCount()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure statement
                queryBuilder.AppendLine("/* Procedure structure for procedure `sp_SP_User_GetCount` */");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_User_GetCount`;");

                // Create procedure statement
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_User_GetCount`() ");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    SELECT COUNT(1) FROM `sp_user`;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_GetCount \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_User_GetPasswordKeyByUserName()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure statement
                queryBuilder.AppendLine("/* Procedure structure for procedure `sp_SP_User_GetPasswordKeyByUserName` */");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_User_GetPasswordKeyByUserName`;");

                // Create procedure statement
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_User_GetPasswordKeyByUserName`(IN p_username VARCHAR(100))");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    SELECT `PasswordKey`");
                queryBuilder.AppendLine("    FROM `sp_user`");
                queryBuilder.AppendLine("    WHERE `UserName` = p_username");
                queryBuilder.AppendLine("    LIMIT 1;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_GetPasswordKeyByUserName \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_User_Insert()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure statement
                queryBuilder.AppendLine("/* Procedure structure for procedure `sp_SP_User_Insert` */");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_User_Insert`;");

                // Create procedure statement
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_User_Insert`(IN p_username VARCHAR(100), IN p_password VARCHAR(250),");
                queryBuilder.AppendLine("    IN p_usertype TINYINT, IN p_active TINYINT, IN p_passwordkey VARCHAR(10), IN p_displayname VARCHAR(50))");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    INSERT INTO `sp_user` (`UserName`, `Password`, `UserType`, `Active`, `PasswordKey`, `CreatedOn`, `DisplayName`)");
                queryBuilder.AppendLine("    VALUES (p_username, p_password, p_usertype, p_active, p_passwordkey, CURRENT_DATE(), p_displayname);");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    SELECT LAST_INSERT_ID();");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_User_IsEmailAvailableForSignup()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure statement
                queryBuilder.AppendLine("/* Procedure structure for procedure `sp_SP_User_IsEmailAvailableForSignup` */");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_User_IsEmailAvailableForSignup`;");

                // Create procedure statement
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_User_IsEmailAvailableForSignup`(IN p_UserEmailId VARCHAR(100))");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    SELECT 1 FROM `sp_user` WHERE `UserName` = p_UserEmailId");
                queryBuilder.AppendLine("    LIMIT 1;");
                queryBuilder.AppendLine("END;");


                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_IsEmailAvailableForSignup \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_User_SignupUser()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure statement
                queryBuilder.AppendLine("/* Procedure structure for procedure `sp_SP_User_SignupUser` */");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_User_SignupUser`;");
                queryBuilder.AppendLine();

                // Create procedure statement
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_User_SignupUser`(IN p_UserEmailId VARCHAR(100), IN p_EncryptedPassword VARCHAR(250), IN p_PasswordKey VARCHAR(10), IN p_VID CHAR(36), IN p_ExpiryTime DATETIME, IN p_TFC INT)");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    DECLARE L_ID INT;");
                queryBuilder.AppendLine("    INSERT INTO `sp_user` (`UserName`, `Password`, `PasswordKey`, `CreatedOn`, `DisplayName`)");
                queryBuilder.AppendLine("    VALUES (p_UserEmailId, p_EncryptedPassword, p_PasswordKey, NOW(), SUBSTRING_INDEX(p_UserEmailId, '@', 1));");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    SET L_ID = LAST_INSERT_ID();");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    INSERT INTO `Verification` (`VID`, `UserID`, `CreatedOn`, `ExpiryTime`, `TFC`)");
                queryBuilder.AppendLine("    VALUES (p_VID, L_ID, NOW(), p_ExpiryTime, p_TFC);");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    SELECT 1;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_SignupUser \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_Student_GetStudentList()
        {
            try
            {

                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("USE `premiersolution_in_01`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Student_GetStudentList`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_Student_GetStudentList`");
                queryBuilder.AppendLine("(");
                queryBuilder.AppendLine("    IN p_PageSize INT,");
                queryBuilder.AppendLine("    IN p_PageNo INT,");
                queryBuilder.AppendLine("    IN p_studentname VARCHAR(50)");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    DECLARE v_Offset INT;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    -- Calculate the offset for pagination");
                queryBuilder.AppendLine("    SET v_Offset = (p_PageNo - 1) * p_PageSize;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    -- Query for total count");
                queryBuilder.AppendLine("    SELECT COUNT(*) AS TotalCount");
                queryBuilder.AppendLine("    FROM sp_Student st");
                queryBuilder.AppendLine("    INNER JOIN sp_User u ON st.UserID = u.UserID");
                queryBuilder.AppendLine("    WHERE (p_studentname IS NULL OR p_studentname = '' OR u.DisplayName LIKE CONCAT('%', p_studentname, '%'));");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    -- Query for paginated results");
                queryBuilder.AppendLine("    SELECT st.DisplayName AS StudentName, s.StandardName, c.CourseName, u.DisplayName AS Email, st.StudentID");
                queryBuilder.AppendLine("    FROM sp_Student st");
                queryBuilder.AppendLine("    INNER JOIN sp_Standard s ON st.StandardID = s.StandardID");
                queryBuilder.AppendLine("    INNER JOIN sp_Course c ON st.CourseID = c.CourseID");
                queryBuilder.AppendLine("    INNER JOIN sp_User u ON st.UserID = u.UserID");
                queryBuilder.AppendLine("    WHERE (p_studentname IS NULL OR p_studentname = '' OR u.DisplayName LIKE CONCAT('%', p_studentname, '%'))");
                queryBuilder.AppendLine("    LIMIT v_Offset, p_PageSize;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_SignupUser \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_sp_standard_Insert()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Standard_Insert`;");

                // CREATE PROCEDURE block
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_Standard_Insert`(IN p_standardname VARCHAR(50), IN p_CreatedBy INT)");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    IF NOT EXISTS (SELECT 1 FROM `sp_Standard` WHERE `StandardName` = p_standardname) THEN");
                queryBuilder.AppendLine("        INSERT INTO `sp_Standard` (`StandardName`, CreatedBy, CreatedOn) VALUES (p_standardname, p_CreatedBy, NOW());");
                queryBuilder.AppendLine("        SELECT LAST_INSERT_ID();");
                queryBuilder.AppendLine("    END IF;");
                queryBuilder.AppendLine("END;");

                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_SignupUser \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_sp_standard_GetStandardList()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Standard_GetStandardList`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_sp_Standard_GetStandardList`() ");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    SELECT `StandardID`, `StandardName` FROM `sp_standard`;");
                queryBuilder.AppendLine("END;");


                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_SignupUser \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Add_sp_SP_Course_GetCourseList()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();


                // Drop procedure if exists
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Course_GetCourseList`;");

                // CREATE PROCEDURE block
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_Course_GetCourseList`() ");
                queryBuilder.AppendLine("BEGIN ");
                queryBuilder.AppendLine("    SELECT `CourseID`, `CourseName` FROM `sp_course`; ");
                queryBuilder.AppendLine("END;");

                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_SignupUser \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public void CreateDefaults()
        {
            #region Tables

            // create ErrorLog table.
            CreateTable_sp_errorlog();

            // create User table
            CreateTable_Sp_User();

            // create Standard table.
            CreateTable_SP_Standard();

            // create Course table.
            CreateTable_sp_course();

            // create Course Videos table.
            CreateTable_sp_coursevideos();

            // create FilesInfo table.
            CreteTable_sp_filesinfo();

            // create Student table.
            CreateTable_sp_student();

            // create RefreshToken table.
            CreteTable_sp_refreshtoken();

            // create Student Activity table.
            CreateTable_sp_studentActivity();

            // create verification table.
            CreteTable_sp_verification();

            #endregion

            #region Procedures
            // Drops if exists and created new procedure for ErrorLog Insert.
            Add_SP_ErrorLog_Insert();

            // Drops if exists and created new procedure for RefreshToken Insert.
            Add_sp_SP_RefreshToken_Upsert();

            // Drops if exists and created new procedure for User Credential check.
            Add_sp_SP_User_CheckCredentials();

            // Drops if exists and created new procedure for returning no of  Users.
            Add_sp_SP_User_GetCount();

            // Drops if exists and created new procedure for returning encrypted password of user.
            Add_sp_SP_User_GetPasswordKeyByUserName();

            // Drops if exists and created new procedure for inserting User.
            Add_sp_SP_User_Insert();

            // Drops if exists and created new procedure for checking user already exist or not.
            Add_sp_SP_User_IsEmailAvailableForSignup();

            // Drops if exists and created new procedure for inserting User and verification table.
            Add_sp_SP_User_SignupUser();

            // Drop if exists and  created new procedure for get studentlist.
            Add_sp_SP_Student_GetStudentList();

            // Drop if exists and create new procedrue for get Standardlist
            Add_sp_sp_standard_GetStandardList();

            // drop if exists and create new procedure for standard insert.
            Add_sp_sp_standard_Insert();

            #endregion

        }

        #endregion


    }
}
