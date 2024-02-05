using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;



//FirebaseAuthentication.SignIn(email,password);
//FirebaseAuthentication.SignUp(email,password);

public class FirebaseAuthentication : MonoBehaviour {

    public JoinPageOpen JPOpen;
    public TMP_InputField LoginEmailInput;
    public TMP_InputField LoginPasswordInput;
    public TMP_InputField JoinEmailInput;
    public TMP_InputField JoinPasswordInput;
    public TMP_InputField PasswordConfirmInput;
    public TMP_InputField NicknameInput;
    public RealtimeDatabase RealtimeDB;
    public TMP_Text ErrorMessage;


  


    private void Start() {
 
    }


    public void SignUp(string email, string password) {
         FirebaseAuth auth;
        auth = FirebaseAuth.DefaultInstance;
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if(task.IsCanceled || task.IsFaulted) {
                Debug.LogError($"Failed to sign up: {task.Exception}");
                return;
            }

            Debug.Log("User signed up successfully!");
        });
    }

    public async Task<int> SignIn(string email, string password) {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        try {
            // 비동기로 로그인 시도
            await auth.SignInWithEmailAndPasswordAsync(email, password);

            // 로그인 성공 시 1 반환
            return 1;
        } catch(FirebaseException e) {
            // 로그인 실패 시 예외 처리
            Debug.LogError($"Failed to sign in: {e.Message}");
            // 로그인 실패 시 0 반환
            return 0;
        }
    }
    public void JoinButtonClick(){
        string email = JoinEmailInput.text;
        string password = JoinPasswordInput.text;
        // 이메일이 유효한지 확인한다
        if(!IsValidEmail(email)) {
            // 유효하지 않은 이메일이면 에러 메시지를 표시한다
            ShowErrorMessage("유효하지 않은 이메일입니다.");
            // 이후의 코드는 실행되지 않도록 리턴한다
            return;
        }

        // 비밀번호가 최소한의 요구 사항을 충족하는지 확인한다
        if(!IsPasswordValid(password)) {
            // 유효하지 않은 비밀번호면 에러 메시지를 표시한다
            ShowErrorMessage("비밀번호가 너무 간단합니다. 특수문자를 포함한 최소한 8자 이상이어야 합니다.");
            // 이후의 코드는 실행되지 않도록 리턴한다
            return;
        }

        // 비밀번호 확인이 일치하는지 확인한다
        if(password != PasswordConfirmInput.text) {
            // 비밀번호 확인이 일치하지 않으면 에러 메시지를 표시한다
            ShowErrorMessage("비밀번호 확인이 일치하지 않습니다.");
            // 이후의 코드는 실행되지 않도록 리턴한다
            return;
        }

        // 닉네임이 유효한지 확인한다
        if(!IsValidNickname(NicknameInput.text)) {
            // 유효하지 않은 닉네임이면 에러 메시지를 표시한다
            ShowErrorMessage("유효하지 않은 닉네임입니다.");
            // 이후의 코드는 실행되지 않도록 리턴한다
            return;
        }
        SignUp(email, password);
    }

    public async void LoginButtonClick() {
        string email = LoginEmailInput.text;
        string password = LoginPasswordInput.text;

        // 로그인 시도
        int result = await SignIn(email, password);

        // 로그인 결과에 따라 처리
        if(result == 1) {
            // 로그인 성공
            Debug.Log("Login successful!");
            LoadScene("SampleScene");
            // 여기에 로그인 성공 후의 추가 동작을 추가할 수 있습니다.
        } else {
            // 로그인 실패
            Debug.LogError("Login failed.");
            // 여기에 로그인 실패 후의 추가 동작을 추가할 수 있습니다.
        }
    }






    // 이메일이 유효한지 확인하는 함수
    // 이메일 유효성 검사
    private bool IsValidEmail(string email) {
        // 간단한 정규 표현식을 사용하여 이메일 형식을 확인합니다
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    // 비밀번호 유효성 검사
    private bool IsPasswordValid(string password) {
        // 비밀번호가 최소 8자 이상이어야 하며, 숫자와 특수 문자를 포함해야 합니다
        return password.Length >= 8 && Regex.IsMatch(password, @"[0-9]") && Regex.IsMatch(password, @"[^a-zA-Z0-9]");
    }

    // 닉네임 유효성 검사
    private bool IsValidNickname(string nickname) {
        // 닉네임은 최소 3자 이상이어야 합니다
        return nickname.Length >= 3;
    }
    // 에러 메시지를 표시하는 함수
    private void ShowErrorMessage(string message) {
        ErrorMessage.text = message;
       
    }
    // 특정 씬을 로드하는 함수
    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}