using System.Data;
using System.Text.RegularExpressions;

namespace Data
{

    public static class UserValdation
    {
        public static void validate<T>(T UsersDto)
        {
            switch (UsersDto)
            {
                case addUserDto adduserdto:
                    validatAddUser(adduserdto);
                    break;
                case updateUserDto updateuserdto:
                    validateUpdateUser(updateuserdto);
                    break;
                //case getUserDto getuserdto:
                //    validatGetUser(getuserdto);
                //    break;
                case deleteUserDto deleteuserdto:
                    validateDeleteUser(deleteuserdto);
                    break;
                case loginUserDto loginuserdto:
                    validateLoginUser(loginuserdto);
                    break;
            }
        }


       
        private static void validateLoginUser(loginUserDto loginuserdto)
        {
            if (string.IsNullOrWhiteSpace(loginuserdto.username))
            {
                throw new InvalidDataException("invalid username");
            }

            if (loginuserdto.password.Length < 8)
            {
                throw new InvalidDataException("Password must be at least 8 characters long.");

            }
        }
        private static void validatAddUser(addUserDto adduserdto)
        {
            if (string.IsNullOrWhiteSpace(adduserdto.username))
            {
                throw new InvalidDataException("invalid username");
            }

            if (adduserdto.password.Length < 8)
            {
                throw new InvalidDataException("Password must be at least 8 characters long.");

            }
            if (!Regex.IsMatch(adduserdto.password, @"[A-Z]"))
            {

                throw new InvalidDataException("Password must contain at least one uppercase letter.");

            }


            if (!Regex.IsMatch(adduserdto.password, @"\d"))
            {
                throw new InvalidDataException("Password must contain at least one number.");

            }

            string[] commonPasswords = { "123456", "password", "qwerty", "admin" };
            if (Array.Exists(commonPasswords, p => p.Equals(adduserdto.password, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidDataException("Password is too common. Please choose a more secure password.");
            }
        }
        private static void validateUpdateUser(updateUserDto updateuserdto)
        {
            if (string.IsNullOrWhiteSpace(updateuserdto.username))
            {
                throw new InvalidDataException("invalid username");
            }
            


            if (updateuserdto.password.Length < 8)
            {
                throw new InvalidDataException("Password must be at least 8 characters long.");

            }
            if (!Regex.IsMatch(updateuserdto.password, @"[A-Z]"))
            {

                throw new InvalidDataException("Password must contain at least one uppercase letter.");

            }


            if (!Regex.IsMatch(updateuserdto.password, @"\d"))
            {
                throw new InvalidDataException("Password must contain at least one number.");

            }

            string[] commonPasswords = { "123456", "password", "qwerty", "admin" };
            if (Array.Exists(commonPasswords, p => p.Equals(updateuserdto.password, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidDataException("Password is too common. Please choose a more secure password.");
            }


        }

        public static void validateDeleteUser(deleteUserDto deleteuserdto)
        {
            if (deleteuserdto.userId < 0)
            {
                throw new InvalidDataException("invalid user id");

            }
        }

    }
    public static class TaskValidation
    {
        public static void validate<T>(T TaskDto)
        {
            switch (TaskDto)
            {
                case addTaskdDto addtaskdto:
                    validatAddTask(addtaskdto);
                    break;
                case updateTaskDto updatetaskdto:
                    validateUpdateTask(updatetaskdto);
                    break;
                case deleteTaskDto deletetaskdto:
                    validateDeleteTask(deletetaskdto);
                    break;
            }
        }

        private static void validatAddTask(addTaskdDto addtaskdto)
        {
            if (string.IsNullOrWhiteSpace(addtaskdto.title))
            {
                throw new InvalidDataException("invalid title");
            }
            if (string.IsNullOrWhiteSpace(addtaskdto.description))
            {
                throw new InvalidDataException("invalid description");
            }
            if (addtaskdto.catgeoryId < 0)
            {
                throw new InvalidDataException("invalid category name ");

            }




        }
        private static void validateUpdateTask(updateTaskDto updatetaskdto)
        {
            if (string.IsNullOrWhiteSpace(updatetaskdto.title))
            {
                throw new InvalidDataException("invalid title");
            }

            if (string.IsNullOrWhiteSpace(updatetaskdto.status))
            {
                throw new InvalidDataException("invalid status");
            }

            if (string.IsNullOrWhiteSpace(updatetaskdto.description))
            {
                throw new InvalidDataException("invalid description");
            }
            if (updatetaskdto.catgeoryId < 0)
            {
                throw new InvalidDataException("invalid category name ");

            }
            if (updatetaskdto.taskId < 0)
            {
                throw new InvalidDataException("invalid task id");
            }
        }
        private static void  validateDeleteTask(deleteTaskDto delettaskdto)
        {
            if (delettaskdto.taskId < 0)
            {
                throw new InvalidDataException("invalid task id");
            }
        }



    }





    public class getTasksDto
    {

        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public int catgeoryId { get; set; }

        public getTasksDto(string title, string description, string status, int catgoryId)
        {

            this.title = title;
            this.description = description;
            this.status = status;
            this.catgeoryId = catgeoryId;
        }


    }
    public class addTaskdDto
    {
        public int taskId { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public int catgeoryId { get; set; }
        public DateTime createdAt { get; set; }


        public addTaskdDto() { }
        public addTaskdDto(int taskId, int userId, string title, string description, string status, int catgoryId, DateTime createdAt)
        {
            this.userId = userId;
            this.taskId = taskId;
            this.title = title;
            this.description = description;
            this.status = status;
            this.catgeoryId = catgeoryId;
            this.createdAt = createdAt;
        }


    }
    public class updateTaskDto
    {
        public int taskId { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public int catgeoryId { get; set; }

        public DateTime updatedAt { get; set; }

        public updateTaskDto() { }
        public updateTaskDto(int taskId, int userId, string title, string description, string status, int catgoryId, DateTime updatedAt)
        {
            this.userId = userId;
            this.taskId = taskId;
            this.title = title;
            this.description = description;
            this.status = status;
            this.catgeoryId = catgeoryId;
            this.updatedAt = updatedAt;
        }


    }

    public class deleteTaskDto
    {
        public int taskId { get; set; }
        public int userId { get; set; }
        public deleteTaskDto() { }
        public deleteTaskDto(int taskId,int userId)
        {
            this.taskId = taskId;
            this.userId = userId;
        }
    }


    public class getUserDto
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }


        public getUserDto(int userId, string username, string firstname, string lastname, DateTime createdat,DateTime updatedat)
        {
            this.userId = userId;
            this.username = username;
            this.firstName = firstname;
            this.lastName = lastname;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
        }

    }

    public class addUserDto
    {

        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        


        public addUserDto(string username, string firstname, string lastname,string password)
           
        {

            this.username = username;
            this.firstName = firstname;
            this.lastName = lastname;
            this.password = password;

        }




    }

    public class updateUserDto
    {


        public int userId { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }


        public updateUserDto(int userId, string username, string firstname, string lastname,string password,bool isactive) 
        {
            this.userId = userId;
            this.username = username;
            this.firstName = firstname;
            this.lastName = lastname;
            this.password = password;
            this.isActive = isactive;
        }




    }
    public class deleteUserDto
    {
        public int userId { get; set; }
        public deleteUserDto() { }
        public deleteUserDto(int userId)
        {
            this.userId = userId;
        }
    }

    public class loginUserDto
    {
        public  string username { get; set; }
        public  string password { get; set; }
        public loginUserDto() { }
        public loginUserDto(string username,string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    


}
