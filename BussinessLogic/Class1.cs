using Data;
using DataAccessLayer;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace BussinessLogic
{


    public static class Users
    {
      
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        public static async Task<getUserDto?> addUser(addUserDto adduserdto)
        {
            try
            {
                UserValdation.validate(adduserdto);
                adduserdto.password = HashPassword(adduserdto.password);
                var newuser = await DataAccess.addUser(adduserdto);
                if (newuser != null)
                {
                    return newuser;
                }
            }
            catch (InvalidDataException err)
            {
                throw new InvalidDataException(err.Message);
            }
            catch (Exception err)
            {
                if (err.Message == "The username you entered already exists. Please use a different one.")
                {
                    throw new Exception("The username you entered already exists. Please use a different one.");
                }

                    throw new Exception(@$"faield adding new user...
                                    {err.Message}");
            }
            return null;



        }

        public static async Task<getUserDto?> getUser(string username)
        {
            try
            {
               
                var User = await DataAccess.getUser(username);
                if (User != null)
                {
                    return User;
                }
            }
            
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            return null;



        }

        public static async Task<getUserDto?> updateUser(updateUserDto updateuserdto)
        {
            try
            {
                UserValdation.validate(updateuserdto);
                updateuserdto.password = HashPassword(updateuserdto.password);
                var User = await DataAccess.updateUser(updateuserdto);
                if (User != null)
                {
                    return User;
                }
            }
            catch (InvalidDataException err)
            {
                throw new InvalidDataException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(@$"faield updating User...
                                    {err.InnerException}");
            }
            return null;



        }
        public static async Task<bool> deleteUser(deleteUserDto deleteuserdto)
        {
            bool deleted = false;
            try
            {
                UserValdation.validate(deleteuserdto);
                deleted = await DataAccess.deleteUser(deleteuserdto.userId);
                deleted = true;
            }
            catch (Exception err)
            {
                throw new Exception(@$"faield deleting User...
                                    {err.Message}");
            }

            return deleted;


        }

        public static async Task<getUserDto> LogUser(string username, string password)
        {
            try
            {
                UserValdation.validate(new loginUserDto( username,  password));

                loginUserDto loguser = await DataAccess.LogedUser(username);
                if (loguser != null)
                {
                    if(VerifyPassword(password, loguser.password))
                    {
                        getUserDto user = await DataAccess.getUser(username);
                        return user;
                    }
                    else {
                        throw new InvalidDataException("incorrect password please try again");
                    }
                }
                throw new InvalidDataException("incorrect username please try again");
            }
            catch (InvalidDataException err)
            {
                throw new InvalidDataException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(@$"login failed 
                                     ${err.Message}");
            }

        }

    }

    public class Tasks
    {




        public static async Task<List<getTasksDto>?> getAllTasks(int userId)
        {
            try
            {
                var tasks = await getAllTasks(userId);


                return tasks;

            }
            catch (Exception err)
            {
                throw new Exception(@$"faield...
                                    {err.Message}");
            }
        }

        public static async Task<addTaskdDto?> addTask(addTaskdDto addtaskdto)
        {
            try
            {
                TaskValidation.validate(addtaskdto);

                var newtask = await DataAccess.addTask(addtaskdto);
                if (newtask != null)
                {
                    return newtask;
                }
            }
            catch (InvalidDataException err)
            {
                throw new InvalidDataException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(@$"faield adding new task...
                                    {err.Message}");
            }
            return null;



        }

        public static async Task<updateTaskDto?> updateTask(updateTaskDto updatetaskdto)
        {
            try
            {
                TaskValidation.validate(updatetaskdto);

                var updatedTask = await DataAccess.updateTask(updatetaskdto);
                if (updatedTask != null)
                {
                    return updatedTask;
                }
            }
            catch (InvalidDataException err)
            {
                throw new InvalidDataException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(@$"faield updating task...
                                    {err.Message}");
            }
            return null;



        }
        public static async Task<bool> deleteTask(deleteTaskDto deletetaskdto)
        {
            bool deleted = false;
            try
            {
                TaskValidation.validate(deletetaskdto);
                deleted = await DataAccess.deleteTask(deletetaskdto.taskId,deletetaskdto.userId);
                deleted = true;
            }
            catch(InvalidDataException err)
            {
                throw new InvalidDataException(err.Message);
            }
            catch (Exception err)
            {
                throw new Exception(@$"faield deleting task...
                                    {err.Message}");
            }

            return deleted;


        }



    }
}
