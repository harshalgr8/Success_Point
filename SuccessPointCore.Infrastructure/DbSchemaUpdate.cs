using Dapper;
using MySqlConnector;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Infrastructure.Interfaces;
using System.Data;
using System.Text;

namespace SuccessPointCore.Infrastructure
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
        public  void CreateTable_sp_errorlog()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
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
        public  void CreateTable_Sp_User()
        {
            try
            {


                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_user` (");
                queryBuilder.AppendLine("`UserID` INT(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("`UserName` VARCHAR(100) NOT NULL,");
                queryBuilder.AppendLine("`Password` VARCHAR(250) NOT NULL,");
                queryBuilder.AppendLine("`UserType` SMALLINT(1) NOT NULL,");
                queryBuilder.AppendLine("`Active` TINYINT(1) NOT NULL DEFAULT 0,");
                queryBuilder.AppendLine("`PasswordKey` VARCHAR(10) DEFAULT NULL,");
                queryBuilder.AppendLine("`CreatedOn` DateTime NOT NULL,");
                queryBuilder.AppendLine("`DisplayName` VARCHAR(50) NULL,");
                queryBuilder.AppendLine("PRIMARY KEY (`UserID`)");
                queryBuilder.AppendLine(") ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                     conn.Execute(queryString, commandType: CommandType.Text);
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
        public  void CreateTable_SP_Standard()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
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
        public  void CreateTable_sp_course()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
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
        public  void CreateTable_sp_coursevideo()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_coursevideo` (");
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
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_coursevideos \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());

            }
        }

        public  void AlterTable_sp_coursevideo_Add_StandardID()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // ALTER TABLE statement
                queryBuilder.AppendLine("ALTER TABLE `sp_coursevideo` ");
                queryBuilder.AppendLine("ADD COLUMN IF NOT Exists `StandardID` int(11);");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_coursevideos \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());

            }
        }

        public  void AlterTable_sp_standard_Add_CreatedBy()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // ALTER TABLE statement
                queryBuilder.AppendLine("ALTER TABLE `sp_standard` ");
                queryBuilder.AppendLine("ADD COLUMN IF NOT Exists `CreatedBy` int(11);");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_coursevideos \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());

            }
        }

        public  void AlterTable_sp_standard_Add_CreatedOn()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // ALTER TABLE statement
                queryBuilder.AppendLine("ALTER TABLE `sp_standard` ");
                queryBuilder.AppendLine("ADD COLUMN IF NOT Exists `CreatedOn` datetime;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_coursevideos \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());

            }
        }

        public  void CreateTable_sp_student()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"table : CreateTable_sp_student \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void CreateTable_sp_studentCourse()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_studentCourse \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void CreateTable_sp_studentActivity()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
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
        public  void CreteTable_sp_filesinfo()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreteTable_sp_filesinfo \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void CreteTable_sp_refreshtoken()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreteTable_sp_refreshtoken \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void CreteTable_sp_verification()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreteTable_sp_verification \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public void AlterTable_sp_Student_Add_CourseIDCsv()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // ALTER TABLE statement
                queryBuilder.AppendLine("ALTER TABLE `sp_Student` ");
                queryBuilder.AppendLine("ADD COLUMN IF NOT Exists `CourseIDCsv` varchar(500) NULL;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_sp_coursevideos \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());

            }
        }


        #endregion

        #region Procedures


        public void Add_SP_ErrorLog_Insert()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_RefreshToken_Upsert()
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

                queryBuilder.AppendLine(" DELETE FROM `sp_refreshtoken` WHERE `UserID` = p_UserID ;");

                queryBuilder.AppendLine("    INSERT INTO `sp_refreshtoken` (`RefreshToken`, `UserID`, `CreatedOn`, `UpdatedOn`, `ExpiresAt`)");
                queryBuilder.AppendLine("    VALUES (p_RefreshToken, p_UserID, NOW(), NULL, p_Token_Expiry_Minute);");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_User_CheckCredentials()
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
                queryBuilder.AppendLine("    WHERE `UserName` = p_Username AND `PASSWORD` = p_EncryptedPassword AND `Active` =1");
                queryBuilder.AppendLine("    LIMIT 1;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_User_GetCount()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_User_GetPasswordKeyByUserName()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_GetPasswordKeyByUserName \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void Add_sp_SP_User_Insert()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void Add_sp_SP_User_IsEmailAvailableForSignup()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_User_SignupUser()
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
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_Student_GetStudentList()
        {
            try
            {

                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine();

                // we added CourseIDCsv column.
                //queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Student_GetStudentList`;");
                //queryBuilder.AppendLine();
                //queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_Student_GetStudentList`");
                //queryBuilder.AppendLine("(");
                //queryBuilder.AppendLine("    IN p_PageSize INT,");
                //queryBuilder.AppendLine("    IN p_PageNo INT,");
                //queryBuilder.AppendLine("    IN p_studentname VARCHAR(50)");
                //queryBuilder.AppendLine(")");
                //queryBuilder.AppendLine("BEGIN");
                //queryBuilder.AppendLine("    DECLARE v_Offset INT;");
                //queryBuilder.AppendLine();
                //queryBuilder.AppendLine("    -- Calculate the offset for pagination");
                //queryBuilder.AppendLine("    SET v_Offset = (p_PageNo - 1) * p_PageSize;");
                //queryBuilder.AppendLine();
                //queryBuilder.AppendLine("    -- Query for total count");
                //queryBuilder.AppendLine("    SELECT COUNT(*) AS TotalCount");
                //queryBuilder.AppendLine("    FROM sp_Student st");
                //queryBuilder.AppendLine("    INNER JOIN sp_User u ON st.UserID = u.UserID");
                //queryBuilder.AppendLine("    WHERE (p_studentname IS NULL OR p_studentname = '' OR u.DisplayName LIKE CONCAT('%', p_studentname, '%'));");
                //queryBuilder.AppendLine();
                //queryBuilder.AppendLine("    -- Query for paginated results");
                //queryBuilder.AppendLine("    SELECT st.DisplayName AS StudentName, s.StandardName,st.CourseIDCsv, c.CourseName, u.UserName AS Email, st.StudentID, u.Active AS IsActive");
                //queryBuilder.AppendLine("    FROM sp_Student st");
                //queryBuilder.AppendLine("    INNER JOIN sp_Standard s ON st.StandardID = s.StandardID");
                //queryBuilder.AppendLine("    INNER JOIN sp_Course c ON st.CourseID = c.CourseID");
                //queryBuilder.AppendLine("    INNER JOIN sp_User u ON st.UserID = u.UserID");
                //queryBuilder.AppendLine("    WHERE (p_studentname IS NULL OR p_studentname = '' OR u.DisplayName LIKE CONCAT('%', p_studentname, '%'))");
                //queryBuilder.AppendLine("    LIMIT v_Offset, p_PageSize;");
                //queryBuilder.AppendLine("END;");


                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Student_GetStudentList`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_Student_GetStudentList`(");
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
                queryBuilder.AppendLine("    SELECT st.DisplayName AS StudentName, s.StandardName,");
                queryBuilder.AppendLine("    IFNULL(GROUP_CONCAT(c.CourseName ORDER BY c.CourseName ASC SEPARATOR ', '), '') AS CourseName,");
                queryBuilder.AppendLine("    u.UserName AS Email, st.StudentID, u.Active AS IsActive, IFNULL(st.CourseIDCsv,'') AS CourseIDCsv ");
                queryBuilder.AppendLine("    FROM sp_Student st");
                queryBuilder.AppendLine("    INNER JOIN sp_Standard s ON st.StandardID = s.StandardID");
                queryBuilder.AppendLine("    LEFT JOIN sp_course c ON FIND_IN_SET(c.CourseID, st.CourseIDCsv) > 0");
                queryBuilder.AppendLine("    INNER JOIN sp_User u ON st.UserID = u.UserID");
                queryBuilder.AppendLine("    WHERE (p_studentname IS NULL OR p_studentname = '' OR u.DisplayName LIKE CONCAT('%', p_studentname, '%'))");
                queryBuilder.AppendLine("    GROUP BY st.StudentID, s.StandardName, st.CourseIDCsv, u.UserName, u.Active");
                queryBuilder.AppendLine("    LIMIT v_Offset, p_PageSize;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_sp_standard_Insert()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Standard_Insert`;");

                

                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_sp_standard_Upsert()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Standard_Upsert`;");

                // Start creating the PROCEDURE definition
                queryBuilder.AppendLine("CREATE  PROCEDURE `sp_SP_Standard_Upsert`(");
                queryBuilder.AppendLine("    IN p_StandardID INT,");
                queryBuilder.AppendLine("    IN p_StandardName VARCHAR(50),");
                queryBuilder.AppendLine("    IN p_Active TINYINT,");
                queryBuilder.AppendLine("    IN p_CreatedBy INT");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");

                // Add the IF-ELSE logic for INSERT or UPDATE
                queryBuilder.AppendLine("    IF p_StandardID = 0 AND NOT EXISTS (SELECT 1 FROM `sp_Standard` WHERE `StandardName` = p_StandardName) THEN");
                queryBuilder.AppendLine("        -- Insert new standard");
                queryBuilder.AppendLine("        INSERT INTO `sp_Standard` (`StandardName`, `Active`, `CreatedBy`, `CreatedOn` )");
                queryBuilder.AppendLine("        VALUES (p_StandardName, p_Active, p_CreatedBy, NOW());");
                queryBuilder.AppendLine("        SELECT LAST_INSERT_ID() AS StandardID;");
                queryBuilder.AppendLine("    ELSE");
                queryBuilder.AppendLine("        -- Update existing standard");
                queryBuilder.AppendLine("        UPDATE `sp_Standard`");
                queryBuilder.AppendLine("        SET `StandardName` = p_StandardName, `Active` = p_Active");
                queryBuilder.AppendLine("        WHERE `StandardID` = p_StandardID;");
                queryBuilder.AppendLine("    END IF;");

                // End of PROCEDURE definition
                queryBuilder.AppendLine("END;");

                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_sp_standard_GetStandardList()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Standard_GetStandardList`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_sp_Standard_GetStandardList`() ");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    SELECT `StandardID`, `StandardName`, `Active` AS isActive FROM `sp_standard`;");
                queryBuilder.AppendLine("END;");


                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_Course_GetCourseList()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();


                // Drop procedure if exists
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Course_GetCourseList`;");

                // CREATE PROCEDURE block
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_Course_GetCourseList`() ");
                queryBuilder.AppendLine("BEGIN ");
                queryBuilder.AppendLine("    SELECT `CourseID`, `CourseName`, `IsActive` FROM `sp_course`; ");
                queryBuilder.AppendLine("END;");

                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_Course_Insert()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();


                // Drop procedure if exists
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Course_Insert`;");

                // CREATE PROCEDURE block
               

                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_Course_Upsert()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();


                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Course_Upsert`;");

                // Start creating the PROCEDURE definition
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_Course_Upsert` (");
                queryBuilder.AppendLine("  IN p_CourseId INT,");
                queryBuilder.AppendLine("  IN p_CourseName VARCHAR(200),");
                queryBuilder.AppendLine("  IN p_IsActive tinyint,");
                queryBuilder.AppendLine("  IN p_CreatedBy INT,");
                queryBuilder.AppendLine("  IN p_ChangedBy INT");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");

                // Add the IF-ELSE logic for INSERT or UPDATE
                queryBuilder.AppendLine("  IF p_CourseId = 0 AND NOT EXISTS (SELECT 1 FROM `sp_course` WHERE `CourseName` = p_CourseName) THEN");
                queryBuilder.AppendLine("    -- Insert new course");
                queryBuilder.AppendLine("    INSERT INTO `sp_course` (");
                queryBuilder.AppendLine("      `CourseName`,");
                queryBuilder.AppendLine("      `IsActive`,");
                queryBuilder.AppendLine("      `CreatedBy`,");
                queryBuilder.AppendLine("      `CreatedOn`,");
                queryBuilder.AppendLine("      `ChangedBy`,");
                queryBuilder.AppendLine("      `ChangedOn`");
                queryBuilder.AppendLine("    )");
                queryBuilder.AppendLine("    VALUES (");
                queryBuilder.AppendLine("      p_CourseName,");
                queryBuilder.AppendLine("      p_IsActive,");
                queryBuilder.AppendLine("      p_CreatedBy,");
                queryBuilder.AppendLine("      NOW(),");
                queryBuilder.AppendLine("      p_CreatedBy,");
                queryBuilder.AppendLine("      NOW()");
                queryBuilder.AppendLine("    );");
                queryBuilder.AppendLine("    SELECT LAST_INSERT_ID() AS CourseID;");
                queryBuilder.AppendLine("  ELSE");
                queryBuilder.AppendLine("    -- Update existing course");
                queryBuilder.AppendLine("    UPDATE `sp_course`");
                queryBuilder.AppendLine("    SET");
                queryBuilder.AppendLine("      `CourseName` = p_CourseName,");
                queryBuilder.AppendLine("      `IsActive` = p_IsActive,");
                queryBuilder.AppendLine("      `ChangedBy` = p_ChangedBy,");
                queryBuilder.AppendLine("      `ChangedOn` = NOW()");
                queryBuilder.AppendLine("    WHERE `CourseID` = p_CourseId;");
                queryBuilder.AppendLine("  END IF;");

                // End of PROCEDURE definition
                queryBuilder.AppendLine("END");

                // CREATE PROCEDURE block


                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_User_GetUserEnrolledCourses()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // now we have csv column so not using course join with student.courseid
                //queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS sp_SP_User_GetUserEnrolledCourses;");
                //queryBuilder.AppendLine();
                //queryBuilder.AppendLine("CREATE PROCEDURE sp_SP_User_GetUserEnrolledCourses(IN p_UserID INT)");
                //queryBuilder.AppendLine("BEGIN");
                //queryBuilder.AppendLine("    SELECT");
                //queryBuilder.AppendLine("        c.CourseName,");
                //queryBuilder.AppendLine("        cv.VideoHeading,");
                //queryBuilder.AppendLine("        cv.VideoName,");
                //queryBuilder.AppendLine("        s.StandardName");
                //queryBuilder.AppendLine("    FROM sp_student st");
                //queryBuilder.AppendLine("    INNER JOIN sp_coursevideo cv");
                //queryBuilder.AppendLine("        ON cv.StandardID = st.StandardID");
                //queryBuilder.AppendLine("        AND cv.CourseID = st.CourseID");
                //queryBuilder.AppendLine("    INNER JOIN sp_standard s");
                //queryBuilder.AppendLine("        ON cv.StandardID = s.StandardID");
                //queryBuilder.AppendLine("    INNER JOIN sp_course c");
                //queryBuilder.AppendLine("        ON cv.CourseID = c.CourseID");
                //queryBuilder.AppendLine("    WHERE st.UserID = p_UserID;");
                //queryBuilder.AppendLine("END;");

                // CREATE PROCEDURE block


                queryBuilder.AppendLine("USE `premiersolution_in_01`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_User_GetUserEnrolledCourses`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("CREATE DEFINER=`premiersolution_01`@`%` PROCEDURE `sp_SP_User_GetUserEnrolledCourses`(IN p_UserID INT)");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    -- Declare a variable to store the CSV of course IDs");
                queryBuilder.AppendLine("    DECLARE course_id_csv VARCHAR(500);");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    -- Fetch the CSV of course IDs for the given user");
                queryBuilder.AppendLine("    SELECT CourseIDCsv INTO course_id_csv");
                queryBuilder.AppendLine("    FROM sp_student");
                queryBuilder.AppendLine("    WHERE UserID = p_UserID;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    -- Ensure the course_id_csv is not NULL or empty");
                queryBuilder.AppendLine("    IF course_id_csv IS NOT NULL AND course_id_csv != '' THEN");
                queryBuilder.AppendLine("        -- Select courses based on CSV");
                queryBuilder.AppendLine("        SELECT");
                queryBuilder.AppendLine("            c.CourseName,");
                queryBuilder.AppendLine("            cv.VideoHeading,");
                queryBuilder.AppendLine("            cv.VideoName,");
                queryBuilder.AppendLine("            s.StandardName");
                queryBuilder.AppendLine("        FROM sp_coursevideo cv");
                queryBuilder.AppendLine("        INNER JOIN sp_standard s");
                queryBuilder.AppendLine("            ON cv.StandardID = s.StandardID");
                queryBuilder.AppendLine("        INNER JOIN sp_course c");
                queryBuilder.AppendLine("            ON FIND_IN_SET(c.CourseID, course_id_csv) > 0");
                queryBuilder.AppendLine("        WHERE cv.CourseID = c.CourseID");
                queryBuilder.AppendLine("        AND cv.StandardID = (SELECT StandardID FROM sp_student WHERE UserID = p_UserID);");
                //queryBuilder.AppendLine("    ELSE");
                //queryBuilder.AppendLine("        -- Return empty result set if no courses found");
                //queryBuilder.AppendLine("        SELECT");
                //queryBuilder.AppendLine("            NULL AS CourseName,");
                //queryBuilder.AppendLine("            NULL AS VideoHeading,");
                //queryBuilder.AppendLine("            NULL AS VideoName,");
                //queryBuilder.AppendLine("            NULL AS StandardName;");
                queryBuilder.AppendLine("    END IF;");
                queryBuilder.AppendLine("END;");


                // To get the complete query string:
                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
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

        public  void Add_sp_SP_Student_UpdateInfo()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure statement
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS sp_SP_Student_UpdateInfo;");
                queryBuilder.AppendLine();

                // now we introduced CourseIDCsv hence not required use of column CourseID
                //Create procedure statement
                queryBuilder.AppendLine("CREATE PROCEDURE sp_SP_Student_UpdateInfo(");
                queryBuilder.AppendLine("    IN p_UserID INT, ");
                queryBuilder.AppendLine("    IN p_StudentID INT, ");
                queryBuilder.AppendLine("    IN p_DisplayName VARCHAR(50), ");
                queryBuilder.AppendLine("    IN p_Username VARCHAR(100), ");
                queryBuilder.AppendLine("    IN p_UserType INT, ");
                queryBuilder.AppendLine("    IN p_Active TINYINT, ");
                queryBuilder.AppendLine("    IN p_StandardID INT, ");
                //queryBuilder.AppendLine("    IN p_CourseID INT");
                queryBuilder.AppendLine("    IN p_CourseIDCsv VARCHAR(500)");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("    IF (p_StudentID > 0) THEN");
                queryBuilder.AppendLine("        UPDATE `sp_student`");
                queryBuilder.AppendLine("        SET `DisplayName` = p_DisplayName, ");
                queryBuilder.AppendLine("            `StandardID` = p_StandardID, ");
                //queryBuilder.AppendLine("            `CourseID` = p_CourseID");
                queryBuilder.AppendLine("            `CourseIDCsv` = p_CourseIDCsv");
                queryBuilder.AppendLine("        WHERE `StudentID` = p_StudentID;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("        UPDATE `sp_user` sp");
                queryBuilder.AppendLine("        INNER JOIN `sp_student` st ON sp.UserID = st.UserID");
                queryBuilder.AppendLine("        SET sp.`UserName` = p_Username,");
                queryBuilder.AppendLine("            sp.`Active` = p_Active");
                queryBuilder.AppendLine("        WHERE st.StudentID = p_StudentID;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("    ELSE");
                //queryBuilder.AppendLine("        INSERT INTO `sp_student` (`UserID`, `DisplayName`, `StandardID`, `CourseID`)");
                //queryBuilder.AppendLine("        VALUES (p_UserID, p_DisplayName, p_StandardID, p_CourseID);");

                queryBuilder.AppendLine("        INSERT INTO `sp_student` (`UserID`, `DisplayName`, `StandardID`, `CourseIDCsv`)");
                queryBuilder.AppendLine("        VALUES (p_UserID, p_DisplayName, p_StandardID, p_CourseIDCsv);");

                queryBuilder.AppendLine("    END IF;");
                queryBuilder.AppendLine("END");



                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void Add_sp_SP_User_UpdatePassword()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure if exists
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_User_UpdatePassword`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("CREATE PROCEDURE `sp_SP_User_UpdatePassword`(");
                queryBuilder.AppendLine("  IN p_UserID INT,");
                queryBuilder.AppendLine("  IN p_Password VARCHAR(250),");
                queryBuilder.AppendLine("  IN p_PasswordKey VARCHAR(10)");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("  UPDATE `sp_user`");
                queryBuilder.AppendLine("  SET `Password` = p_Password,");
                queryBuilder.AppendLine("      `PasswordKey` = p_PasswordKey");
                queryBuilder.AppendLine("  WHERE `UserID` = p_UserID;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void Add_sp_SP_Student_UpdatePassword()
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                // Drop procedure if exists
                queryBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_SP_Student_UpdatePassword`;");
                queryBuilder.AppendLine();
                queryBuilder.AppendLine("CREATE  PROCEDURE `sp_SP_Student_UpdatePassword`(");
                queryBuilder.AppendLine("  IN p_StudentID INT,");
                queryBuilder.AppendLine("  IN p_Password VARCHAR(250),");
                queryBuilder.AppendLine("  IN p_PasswordKey VARCHAR(10)");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("BEGIN");
                queryBuilder.AppendLine("   -- Update the password and password key in sp_user using a JOIN with sp_student");
                queryBuilder.AppendLine("  UPDATE `sp_user` u");
                queryBuilder.AppendLine("  JOIN `sp_student` s ON u.`UserID` = s.`UserID`");
                queryBuilder.AppendLine("  SET u.`Password` = p_Password,");
                queryBuilder.AppendLine("      u.`PasswordKey` = p_PasswordKey");
                queryBuilder.AppendLine("  WHERE s.`StudentID` = p_StudentID;");
                queryBuilder.AppendLine("END;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void Add_sp_sp_courseVideo_Upsert() {
            try
            {
                StringBuilder sqlBuilder = new StringBuilder();

                // without removing course that not present in json
                // Drop procedure if exists
                //sqlBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_sp_courseVideo_Upsert`;");
                //sqlBuilder.AppendLine("");
                //sqlBuilder.AppendLine("CREATE PROCEDURE `sp_sp_courseVideo_Upsert`(IN p_course_list JSON)");
                //sqlBuilder.AppendLine("BEGIN");
                //sqlBuilder.AppendLine("    DECLARE v_video_id INT;");
                //sqlBuilder.AppendLine("    DECLARE v_course_id INT;");
                //sqlBuilder.AppendLine("    DECLARE v_standard_id INT;");
                //sqlBuilder.AppendLine("    DECLARE v_video_name VARCHAR(255);");
                //sqlBuilder.AppendLine("    DECLARE v_video_heading VARCHAR(255);");
                //sqlBuilder.AppendLine("    DECLARE v_order INT;");
                //sqlBuilder.AppendLine("    DECLARE done INT DEFAULT 0;");
                //sqlBuilder.AppendLine("    DECLARE cur CURSOR FOR ");
                //sqlBuilder.AppendLine("        SELECT ");
                //sqlBuilder.AppendLine("            JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.VideoID')) AS VideoID,");
                //sqlBuilder.AppendLine("            JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.CourseID')) AS CourseID,");
                //sqlBuilder.AppendLine("            JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.StandardID')) AS StandardID,");
                //sqlBuilder.AppendLine("            JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.VideoName')) AS VideoName,");
                //sqlBuilder.AppendLine("            JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.VideoHeading')) AS VideoHeading,");
                //sqlBuilder.AppendLine("            JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.Order')) AS `Order`");
                //sqlBuilder.AppendLine("        FROM JSON_TABLE(p_course_list, '$[*]' COLUMNS(VALUE JSON PATH '$')) j;");
                //sqlBuilder.AppendLine("    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;");
                //sqlBuilder.AppendLine();
                //sqlBuilder.AppendLine("    OPEN cur;");
                //sqlBuilder.AppendLine("    read_loop: LOOP");
                //sqlBuilder.AppendLine("        FETCH cur INTO v_video_id, v_course_id, v_standard_id, v_video_name, v_video_heading, v_order;");
                //sqlBuilder.AppendLine("        IF done THEN");
                //sqlBuilder.AppendLine("            LEAVE read_loop;");
                //sqlBuilder.AppendLine("        END IF;");
                //sqlBuilder.AppendLine();
                //sqlBuilder.AppendLine("        -- If VideoID is greater than 0, update the existing record");
                //sqlBuilder.AppendLine("        IF v_video_id > 0 THEN");
                //sqlBuilder.AppendLine("            UPDATE sp_coursevideo");
                //sqlBuilder.AppendLine("            SET VideoName = v_video_name,");
                //sqlBuilder.AppendLine("                VideoHeading = v_video_heading,");
                //sqlBuilder.AppendLine("                `Order` = v_order");
                //sqlBuilder.AppendLine("            WHERE VideoID = v_video_id;");
                //sqlBuilder.AppendLine("        ELSE");
                //sqlBuilder.AppendLine("            -- Insert a new record if VideoID is not greater than 0");
                //sqlBuilder.AppendLine("            INSERT INTO sp_coursevideo (CourseID, StandardID, VideoName, VideoHeading, `Order`)");
                //sqlBuilder.AppendLine("            VALUES (v_course_id, v_standard_id, v_video_name, v_video_heading, v_order);");
                //sqlBuilder.AppendLine("        END IF;");
                //sqlBuilder.AppendLine();
                //sqlBuilder.AppendLine("    END LOOP;");
                //sqlBuilder.AppendLine();
                //sqlBuilder.AppendLine("    CLOSE cur;");
                //sqlBuilder.AppendLine("END;");
                //sqlBuilder.AppendLine();


                //with removing course that not present in json for Course_ID and Standard_ID

                sqlBuilder.AppendLine("USE `premiersolution_in_01`;");
                sqlBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_sp_courseVideo_Upsert`;");
                sqlBuilder.AppendLine("CREATE DEFINER=`premiersolution_01`@`%` PROCEDURE `sp_sp_courseVideo_Upsert`(IN p_course_list JSON)");
                sqlBuilder.AppendLine("BEGIN");
                sqlBuilder.AppendLine("    DECLARE v_video_id INT;");
                sqlBuilder.AppendLine("    DECLARE v_course_id INT;");
                sqlBuilder.AppendLine("    DECLARE v_standard_id INT;");
                sqlBuilder.AppendLine("    DECLARE v_video_name VARCHAR(255);");
                sqlBuilder.AppendLine("    DECLARE v_video_heading VARCHAR(255);");
                sqlBuilder.AppendLine("    DECLARE v_order INT;");
                sqlBuilder.AppendLine("    DECLARE done INT DEFAULT 0;");

                sqlBuilder.AppendLine("    DECLARE cur CURSOR FOR");
                sqlBuilder.AppendLine("        SELECT");
                sqlBuilder.AppendLine("            CAST(JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.VideoID')) AS UNSIGNED) AS VideoID,");
                sqlBuilder.AppendLine("            CAST(JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.CourseID')) AS UNSIGNED) AS CourseID,");
                sqlBuilder.AppendLine("            CAST(JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.StandardID')) AS UNSIGNED) AS StandardID,");
                sqlBuilder.AppendLine("            JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.VideoName')) AS VideoName,");
                sqlBuilder.AppendLine("            JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.VideoHeading')) AS VideoHeading,");
                sqlBuilder.AppendLine("            CAST(JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.Order')) AS UNSIGNED) AS `Order`");
                sqlBuilder.AppendLine("        FROM JSON_TABLE(p_course_list, '$[*]' COLUMNS(value JSON PATH '$')) j;");

                sqlBuilder.AppendLine("    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;");

                sqlBuilder.AppendLine("    SET v_course_id = CAST(JSON_UNQUOTE(JSON_EXTRACT(p_course_list, '$[0].CourseID')) AS UNSIGNED);");
                sqlBuilder.AppendLine("    SET v_standard_id = CAST(JSON_UNQUOTE(JSON_EXTRACT(p_course_list, '$[0].StandardID')) AS UNSIGNED);");

                sqlBuilder.AppendLine("    DELETE FROM sp_coursevideo");
                sqlBuilder.AppendLine("    WHERE CourseID = v_course_id");
                sqlBuilder.AppendLine("      AND StandardID = v_standard_id");
                sqlBuilder.AppendLine("      AND VideoID NOT IN (");
                sqlBuilder.AppendLine("          SELECT CAST(JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.VideoID')) AS UNSIGNED)");
                sqlBuilder.AppendLine("          FROM JSON_TABLE(p_course_list, '$[*]' COLUMNS(value JSON PATH '$')) j");
                sqlBuilder.AppendLine("          WHERE JSON_UNQUOTE(JSON_EXTRACT(j.value, '$.VideoID')) IS NOT NULL");
                sqlBuilder.AppendLine("      );");

                sqlBuilder.AppendLine("    OPEN cur;");
                sqlBuilder.AppendLine("    read_loop: LOOP");
                sqlBuilder.AppendLine("        FETCH cur INTO v_video_id, v_course_id, v_standard_id, v_video_name, v_video_heading, v_order;");
                sqlBuilder.AppendLine("        IF done THEN");
                sqlBuilder.AppendLine("            LEAVE read_loop;");
                sqlBuilder.AppendLine("        END IF;");

                sqlBuilder.AppendLine("        IF v_video_id > 0 THEN");
                sqlBuilder.AppendLine("            UPDATE sp_coursevideo");
                sqlBuilder.AppendLine("            SET VideoName = v_video_name,");
                sqlBuilder.AppendLine("                VideoHeading = v_video_heading,");
                sqlBuilder.AppendLine("                `Order` = v_order,");
                sqlBuilder.AppendLine("                `CourseID` = v_course_id");
                sqlBuilder.AppendLine("            WHERE VideoID = v_video_id;");
                sqlBuilder.AppendLine("        ELSE");
                sqlBuilder.AppendLine("            INSERT INTO sp_coursevideo (CourseID, StandardID, VideoName, VideoHeading, `Order`)");
                sqlBuilder.AppendLine("            VALUES (v_course_id, v_standard_id, v_video_name, v_video_heading, v_order);");
                sqlBuilder.AppendLine("        END IF;");

                sqlBuilder.AppendLine("    END LOOP;");

                sqlBuilder.AppendLine("    CLOSE cur;");
                sqlBuilder.AppendLine("END;");

                string queryString = sqlBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void Add_sp_sp_CourseVideo_GetCourse() {
            try
            {
                StringBuilder sqlBuilder = new StringBuilder();

                // Drop the procedure if it exists
                sqlBuilder.AppendLine("DROP PROCEDURE IF EXISTS sp_sp_CourseVideo_GetCourse;");

                // Create the new procedure
                sqlBuilder.AppendLine("CREATE PROCEDURE sp_sp_CourseVideo_GetCourse(IN p_courseId INT, IN p_standardId INT)");
                sqlBuilder.AppendLine("BEGIN");
                sqlBuilder.AppendLine("    SELECT");
                sqlBuilder.AppendLine("        VideoID,");
                sqlBuilder.AppendLine("        VideoHeading,");
                sqlBuilder.AppendLine("        VideoName,");
                sqlBuilder.AppendLine("        sp_coursevideo.CourseID,");
                sqlBuilder.AppendLine("        sp_coursevideo.StandardID,");
                sqlBuilder.AppendLine("        sp_course.CourseName,");
                sqlBuilder.AppendLine("        sp_standard.StandardName");
                sqlBuilder.AppendLine("    FROM sp_coursevideo");
                sqlBuilder.AppendLine("    INNER JOIN sp_course ON sp_coursevideo.CourseID = sp_course.CourseID");
                sqlBuilder.AppendLine("    INNER JOIN sp_standard ON sp_coursevideo.StandardID = sp_standard.StandardID");
                //sqlBuilder.AppendLine("    WHERE sp_course.CourseID = p_courseId");
                sqlBuilder.AppendLine("    WHERE sp_course.CourseID = CASE WHEN p_courseId > 0 THEN p_courseId ELSE sp_course.CourseID END");
                sqlBuilder.AppendLine("      AND sp_standard.StandardID = p_standardId;");
                sqlBuilder.AppendLine("END;");

                string queryString = sqlBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public  void Add_sp_sp_CourseVideo_GetStandardwiseUniqueCourse() {
            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                // Drop the procedure if it exists
                sqlBuilder.AppendLine("DROP PROCEDURE IF EXISTS sp_sp_CourseVideo_GetStandardwiseUniqueCourse;");

                // Create the new procedure
                sqlBuilder.AppendLine("CREATE PROCEDURE sp_sp_CourseVideo_GetStandardwiseUniqueCourse()");
                sqlBuilder.AppendLine("BEGIN");
                sqlBuilder.AppendLine("    SELECT DISTINCT");
                sqlBuilder.AppendLine("        sp_coursevideo.CourseID,");
                sqlBuilder.AppendLine("        sp_coursevideo.StandardID,");
                sqlBuilder.AppendLine("        sp_course.CourseName,");
                sqlBuilder.AppendLine("        sp_standard.StandardName,  0 AS `VideoID`, '' AS `VideoHeading`,'' AS `VideoName`");
                sqlBuilder.AppendLine("    FROM sp_coursevideo");
                sqlBuilder.AppendLine("    INNER JOIN sp_course ON sp_coursevideo.CourseID = sp_course.CourseID");
                sqlBuilder.AppendLine("    INNER JOIN sp_standard ON sp_coursevideo.StandardID = sp_standard.StandardID;");
                sqlBuilder.AppendLine("END;");

                string queryString = sqlBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                     conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public void Add_sp_sp_CourseVideo_remove_coursevideo() {

            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                // Drop the procedure if it exists
                sqlBuilder.AppendLine("DROP PROCEDURE IF EXISTS `sp_sp_CourseVideo_remove_coursevideo`;");
                sqlBuilder.AppendLine("CREATE PROCEDURE `sp_sp_CourseVideo_remove_coursevideo`(IN p_CourseId INT, IN p_StandardId INT)");
                sqlBuilder.AppendLine("BEGIN");
                sqlBuilder.AppendLine("    DELETE FROM `sp_coursevideo`");
                sqlBuilder.AppendLine("    WHERE `CourseID` = p_CourseId");
                sqlBuilder.AppendLine("      AND `StandardID` = p_StandardId;");
                sqlBuilder.AppendLine("END;");

                string queryString = sqlBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public void Add_sp_sp_Student_Delete()
        {

            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                // Drop the procedure if it exists
                sqlBuilder.AppendLine("DROP PROCEDURE IF EXISTS sp_sp_Student_Delete;");
                sqlBuilder.AppendLine();
                sqlBuilder.AppendLine("CREATE PROCEDURE sp_sp_Student_Delete(IN p_StudentId INT)");
                sqlBuilder.AppendLine("BEGIN");
                sqlBuilder.AppendLine("    DECLARE v_UserId INT;");
                sqlBuilder.AppendLine();
                sqlBuilder.AppendLine("    -- Get the UserID associated with the StudentID");
                sqlBuilder.AppendLine("    SELECT UserID INTO v_UserId");
                sqlBuilder.AppendLine("    FROM sp_student");
                sqlBuilder.AppendLine("    WHERE StudentID = p_StudentId;");
                sqlBuilder.AppendLine();
                sqlBuilder.AppendLine("    -- Delete from sp_student table");
                sqlBuilder.AppendLine("    DELETE FROM sp_student");
                sqlBuilder.AppendLine("    WHERE StudentID = p_StudentId;");
                sqlBuilder.AppendLine();
                sqlBuilder.AppendLine("    -- Delete from verification table");
                sqlBuilder.AppendLine("    DELETE FROM verification");
                sqlBuilder.AppendLine("    WHERE UserID = v_UserId;");
                sqlBuilder.AppendLine();
                sqlBuilder.AppendLine("    -- Delete from sp_student_activity table");
                sqlBuilder.AppendLine("    DELETE FROM sp_student_activity");
                sqlBuilder.AppendLine("    WHERE UserID = v_UserId;");

                sqlBuilder.AppendLine("    -- Delete from sp_refreshtoken table");
                sqlBuilder.AppendLine("    DELETE FROM sp_refreshtoken");
                sqlBuilder.AppendLine("    WHERE UserID = v_UserId;");

                sqlBuilder.AppendLine();
                sqlBuilder.AppendLine("    -- Delete from sp_user table");
                sqlBuilder.AppendLine("    DELETE FROM sp_user");
                sqlBuilder.AppendLine("    WHERE UserID = v_UserId;");
                sqlBuilder.AppendLine();
                sqlBuilder.AppendLine("END;");

                string queryString = sqlBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                ErrorLogRepository.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                Console.WriteLine(ex.ToString());
            }
        }

        public void Add_sp_sp_Standard_Delete()
        {

            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                // Drop the procedure if it exists
                sqlBuilder.AppendLine("DROP PROCEDURE IF EXISTS sp_sp_Standard_Delete;");
                sqlBuilder.AppendLine("CREATE PROCEDURE sp_sp_Standard_Delete(IN p_StandardId INT)");
                sqlBuilder.AppendLine("BEGIN");
                sqlBuilder.AppendLine("    DELETE FROM sp_standard WHERE StandardID = p_StandardId;");
                sqlBuilder.AppendLine("END;");

                string queryString = sqlBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    conn.Open();
                    conn.Execute(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                //await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"procedure : Add_sp_SP_User_Insert \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
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
            CreateTable_sp_coursevideo();

            // Alter sp_coursevideo add StandardID column.
            AlterTable_sp_coursevideo_Add_StandardID();

            // alter table sp_standard add createdby column
            AlterTable_sp_standard_Add_CreatedBy();

            // alter table sp_standard_createdOn
            AlterTable_sp_standard_Add_CreatedOn();
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


            AlterTable_sp_Student_Add_CourseIDCsv();

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

            // Drop if exists and create new procedrue for get Standardlist.
            Add_sp_sp_standard_GetStandardList();

            // drop if exists and create new procedure for standard insert.
            Add_sp_sp_standard_Insert();

            // Drop if exist and create new procedure for standard upsert.
            Add_sp_sp_standard_Upsert();

            // drop if exists and create new procedure for CourseList.
            Add_sp_SP_Course_GetCourseList();

            // drop if exists and create new procedure for Course insert.
            Add_sp_SP_Course_Insert();

            // Upsert Course
            Add_sp_SP_Course_Upsert();


            // drop if exists and create new procedure for enrolled courses
            Add_sp_SP_User_GetUserEnrolledCourses();

            // drop if exist and create new procedure for SP_Student_UpdateInfo
            Add_sp_SP_Student_UpdateInfo();

            // drop if exist and create new procedure for SP_User_UpdatePassword
            Add_sp_SP_User_UpdatePassword();

            Add_sp_sp_courseVideo_Upsert();

            Add_sp_sp_CourseVideo_GetCourse();

            Add_sp_sp_CourseVideo_GetStandardwiseUniqueCourse();

            Add_sp_sp_CourseVideo_remove_coursevideo();

            Add_sp_sp_Student_Delete();

            Add_sp_sp_Standard_Delete();

            #endregion

        }

        #endregion


    }
}
