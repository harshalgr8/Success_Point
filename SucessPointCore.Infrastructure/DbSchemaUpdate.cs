using Dapper;
using MySqlConnector;
using SucessPointCore.Domain.Constants;
using System.Data;
using System.Text;

namespace SucessPointCore.Infrastructure
{
    public class DbSchemaUpdate
    {
        private string DbConnectionString { get; set; }

        public DbSchemaUpdate(string dbConnectionString)
        {
            DbConnectionString = dbConnectionString;
        }

        #region Tables

        /// <summary>
        /// It creates ErrorLog table which will store all the information related to erros.
        /// </summary>
        public async void CreateTable_sp_errorlog() {
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
                await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_Sp_User \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
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
                await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_Sp_User \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
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
                await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_Sp_User \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// It creates Course table which will store course that student can enroll if needed.
        /// </summary>
        public async void CreateTable_sp_course() {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("CREATE TABLE IF NOT EXISTS `sp_course` (");
                queryBuilder.AppendLine("`CourseID` INT(11) NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("`CourseName` VARCHAR(200) NOT NULL,");
                queryBuilder.AppendLine("`IsActive` TINYINT(1) NOT NULL,");
                queryBuilder.AppendLine("`UserID` INT(11) DEFAULT NULL,");
                queryBuilder.AppendLine("PRIMARY KEY (`CourseID`),");
                queryBuilder.AppendLine("KEY `UserID` (`UserID`)");
                queryBuilder.AppendLine(") ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;");

                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_Sp_User \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// It creates coursevideos table that store information of videos of various courses
        /// </summary>
        public async void CreateTable_sp_coursevideos() {
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
                await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_Sp_User \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                Console.WriteLine(ex.ToString());
            }
        }

        public async void CreateTable_sp_student()
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendLine($"CREATE TABLE IF NOT EXISTS `sp_student` (");
                queryBuilder.AppendLine("    `StudentID` INT NOT NULL AUTO_INCREMENT,");
                queryBuilder.AppendLine("    `UserID` INT NOT NULL,");
                queryBuilder.AppendLine("    `DisplayName` VARCHAR(255) NOT NULL,");
                queryBuilder.AppendLine("    `StandardID` INT NOT NULL,");
                queryBuilder.AppendLine("    `CourseID` INT NOT NULL,");
                queryBuilder.AppendLine("    PRIMARY KEY (`StudentID`)");
                queryBuilder.AppendLine(") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");


                string queryString = queryBuilder.ToString();

                using (IDbConnection conn = new MySqlConnection(connectionString: DbConnectionString))
                {
                    await conn.ExecuteAsync(queryString, commandType: CommandType.Text);
                }
            }
            catch (MySqlException ex)
            {
                await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_Sp_User \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                Console.WriteLine(ex.ToString());
            }
        }

        //It creates FileInfo table which stores information of videos uploaded on system by admin
        public async void CreteTable_sp_filesinfo() {

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
                await File.AppendAllTextAsync(FileConstant.SqlErrorLogFile, $"function : CreateTable_Sp_User \tError :{ex.Message}\tStackTrace :{ex.StackTrace}\r\n");
                Console.WriteLine(ex.ToString());
            }
        }


        #endregion

        #region Procedures

        public void Add_SP_User_Insert()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
        }

        
        public void Update_SP_User_Insert() { }

        public void Add_SP_User_UpdateInfo() { }

        public void Update_SP_User_UpdateInfo() { }

        public void Add_SP_User_UpdatePassword() { }

        public void Update_SP_User_UpdatePassword() { }

        public void Add_SP_User_GetCount() { }

        public void Add_SP_Users_GetPasswordKeyByUserName() { }

        public void Update_SP_Users_GetPasswordKeyByUserName() { }
        public void Add_SP_Users_CheckCredentials() { }
        public void Update_SP_Users_CheckCredentials() { }
        public void Add_SP_User_GetUserEnrolledCourses() { }

        public void Update_SP_User_GetUserEnrolledCourses() { }

        public void Add_SP_ErrorLog_Insert() { }

        public void CreateDefaults()
        {
            #region Tables

            // create ErrorLog table
            CreateTable_sp_errorlog();

            // create User table
            CreateTable_Sp_User();

            // create Standard table
            CreateTable_SP_Standard();

            // create Course table
            CreateTable_sp_course();

            // create Course Videos table
            CreateTable_sp_coursevideos();

            // create FilesInfo table
            CreteTable_sp_filesinfo();
            
            // create Student table
            CreateTable_sp_student();

            #endregion

            #region Procedures

            #endregion

        }

        #endregion


    }
}
