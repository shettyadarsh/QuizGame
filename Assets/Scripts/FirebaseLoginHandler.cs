using System;
using System.Collections;
using System.Collections.Generic;
using Google;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Net.Http;
using Firebase.Extensions;
using Firebase.Auth;
using System.Threading.Tasks;
using Firebase;

public class FirebaseLoginHandler : MonoBehaviour
{
       // Google Web API client ID for authentication
    public string GooglwebApi = "182059864784-13ntdogv7h0884t5627a9unvqmj3n0ia.apps.googleusercontent.com";

    // Configuration for Google Sign-In
    private GoogleSignInConfiguration configuration;

    // Firebase dependencies and authentication objects
    private Firebase.DependencyStatus _dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;

    // UI elements to display user information
    public TMP_Text UsernameText, UserEmailText;
    public Image profilePicture;
    public string imageUrl;

    // UI screens for login and profile display
    public GameObject loginScreen, ProfileScreen;

    private void Awake()
    {
        // Configure webApi with Google
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GooglwebApi,
            RequestIdToken = true
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize Firebase
        InitFirebase();
       // GoogleSignIn.DefaultInstance.SignOut();
    }

    private void InitFirebase()
    {
        // Initialize Firebase authentication
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // Triggered when the Google Sign-In button is clicked
    public void GoogleSignInClick()
    {
        // Sign out from Google (to ensure a new sign-in)
        //GoogleSignIn.DefaultInstance.SignOut();
        
        // Set up Google Sign-In configuration
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        // Initiate Google Sign-In
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticationFinished);
    }

    // Callback function when Google Sign-In is finished
    void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            // Handle authentication fault
            Debug.LogError("Authentication fault");
        }
        else if (task.IsCanceled)
        {
            // Handle authentication cancellation
            Debug.LogError("Login canceled");
        }
        else
        {
            // Retrieve Google Sign-In user's credentials
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            // Sign in with Firebase using Google credentials
            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(authTask =>
            {
                if (authTask.IsCanceled)
                {
                    // Handle Firebase authentication cancellation
                    Debug.LogError("SignInWithCredential was cancelled");
                    return;
                }

                if (authTask.IsFaulted)
                {
                    // Handle Firebase authentication error
                    Debug.LogError("SignInWithCredentialasync encountered an error: " + authTask.Exception);
                    return;
                }

                // Successfully authenticated user
                user = auth.CurrentUser;

                // Update UI with user information
                UsernameText.text = user.DisplayName;
                UserEmailText.text = user.Email;
                loginScreen.SetActive(false);
                ProfileScreen.SetActive(true);

                // Load and display user profile picture
                StartCoroutine(LoadImage(CheckImageurl(user.PhotoUrl.ToString())));
            });
        }
    }

    // Check if the image URL is valid, otherwise use a default URL
    private string CheckImageurl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }

        return imageUrl;
    }
    
    public void Logout()
    {
        // Sign out from Firebase authentication
        auth.SignOut();
        
        if (auth.CurrentUser == null)
        {
            Debug.Log("User successfully logged out.");
            GoogleSignIn.DefaultInstance.SignOut();
        }
        else
        {
            Debug.LogWarning("User is still logged in. This may happen due to an asynchronous operation.");
        }
        
        // Clear UI elements and switch back to the login screen
        UsernameText.text = "";
        UserEmailText.text = "";
        profilePicture.sprite = null;
        
        loginScreen.SetActive(true);
        ProfileScreen.SetActive(false);
    }

    // Coroutine to load and display user profile picture
    IEnumerator LoadImage(string imageUrl)
    {
        WWW www = new WWW(imageUrl);
        yield return www;

        // Create a sprite from the loaded texture and set it to the profile picture
        profilePicture.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
            new Vector2(0, 0));
    }
}
