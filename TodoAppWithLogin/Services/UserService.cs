//using TodoAppWithLogin.Data;
//using TodoAppWithLogin.Models;

//namespace TodoAppWithLogin.Services
//{
//    public class UserService : IUserService
//    {
//        private static readonly List<(string Username, string Password)> Users = new();
//        private AppDbContext _context;

//        public UserService(AppDbContext context)
//        {
//            _context = context;
//        }
//        public bool Register(string firstName, string lastName, string username, string password)
//        {
//            if (_context.Users.Any(u => u.Username == username))
//                return false;

//            _context.Users.Add(new Users { FirstName = firstName, LastName = lastName, Username = username, Password = password });
//            _context.SaveChanges();
//            return true;
//        }

//        public bool ValidateUser(string username, string password)
//        {
//            return _context.Users.Any(u => u.Username == username && u.Password == password);
//        }

//        public Users? Get(string userName) => _context.Users.FirstOrDefault(u => u.Username == userName);

//        // public IEnumerable<Todo> GetTodos(string username) => _context.Users.Where(u => u.Username == username).SelectMany(u => u.Todos);

//        public List<Users> GetAll() => _context.Users.ToList();
//    }
//}
