using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LabWork7.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    AppDbContext context;
    UserService userAppService;

    [ObservableProperty]
    private string newUserLogin;

    [ObservableProperty]
    private string newUserPassword;

    [ObservableProperty]
    private User selectedUser;

    [ObservableProperty]
    private ObservableCollection<User> usersList = new ObservableCollection<User>();

    public bool IsEditing => SelectedUser != null;

    public MainViewModel()
    {
        context = new AppDbContext();
        userAppService = new(context);
        // Загрузите пользователей при создании ViewModel
        LoadUsers().Wait();
    }


    public async Task LoadUsers()
    {
        UsersList = [.. await userAppService.GetUsersAsync()];
    }

    [RelayCommand]
    public async Task AddUser()
    {
        await userAppService.CreateUserAsync(NewUserLogin, NewUserPassword);

        await LoadUsers();
    }

    [RelayCommand]
    public async Task DeleteUser(User user)
    {
        await userAppService.DeleteUserAsync(user.Id);

        await LoadUsers();
    }

    [RelayCommand]
    public async Task UpdateUser()
    {
        if(SelectedUser == null)
            return;

        SelectedUser.Login = NewUserLogin;
        SelectedUser.Password = NewUserPassword;

        await userAppService.ChangeUserAsync(SelectedUser.Id, SelectedUser.Login, SelectedUser.Password);

        var index = UsersList.IndexOf(SelectedUser);
        UsersList[index] = SelectedUser;

        SelectedUser = null;
        ClearFields();
    }

    [RelayCommand]
    private void EditUser(User user) 
    {
        SelectedUser = user;
        NewUserLogin = user.Login;
        NewUserPassword = user.Password;
        OnPropertyChanged(nameof(IsEditing));
    }

    [RelayCommand]
    private void CancelEdit()
    {
        SelectedUser = null;
        ClearFields();
    }

    [RelayCommand]
    private void ClearFields() 
    {
        SelectedUser = null;
        NewUserLogin = null;
        OnPropertyChanged(nameof(IsEditing));
    }

    //[RelayCommand]
    //private async Task CreateUser()
    //{
    //    // 1. Дожидаемся завершения создания пользователя
    //    await userAppService.CreateUserAsync(NewUserLogin, NewUserPassword);

    //    // 2. Дожидаемся завершения обновления списка пользователей
    //    await LoadUsersAsync();
    //}

    //private ObservableCollection<User> _userList = new() { new User { Id = 1, Login = "Goida", Password = "Password123" } };

    //public ObservableCollection<User> UserList
    //{
    //    get => _userList;
    //    private set => SetProperty(ref _userList, value);
    //}

    //public async Task LoadUsersAsync()
    //{
    //    var users = await userAppService.GetUsersAsync();
    //    UserList = new ObservableCollection<User>(users);
    //}
}
