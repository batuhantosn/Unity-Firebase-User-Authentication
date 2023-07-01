using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;

public class LoginManager : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public Button loginButton;
        public Button registerButton;
    public Button resetPasswordButton;
    
    private FirebaseAuth auth;
    
    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase with error: " + task.Result.ToString());
            }
        });
        
        // Attach login function to the login button
        loginButton.onClick.AddListener(Login);

        // Attach register function to the register button
        registerButton.onClick.AddListener(Register);

        // Attach reset password function to the reset password button
        resetPasswordButton.onClick.AddListener(ResetPassword);
    }
    
    void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        
        // Sign in the user with email and password
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            
            // User successfully logged in
            AuthResult result = task.Result;
            FirebaseUser user = result.User;
            Debug.Log("User logged in: " + user.Email);
        });
    }
    void Register()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // Create a new user with email and password
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // User registration successful
            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            Debug.Log("New user registered: " + newUser.Email);
        });
    }

    void ResetPassword()
    {
        string email = emailInput.text;

        // Send a password reset email to the user's email address
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                return;
            }

            // Password reset email sent successfully
            Debug.Log("Password reset email sent to: " + email);
        });
    }
}
