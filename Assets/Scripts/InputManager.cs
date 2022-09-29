////Decompile from assembly: Assembly-CSharp.dll
//using System;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using System.Threading;
//using UnityEngine;

//public class InputManager : MonoBehaviour
//{
//    public delegate void AttackAction(int side);

//    public static InputManager instance;

//    private float width;

//    private float height;



//    public static event InputManager.AttackAction OnAttackPressed;

//    private void Awake()
//    {
//        InputManager.instance = this;
//        this.width = (float)Screen.width;
//        this.height = (float)Screen.height;
//    }

//    private void Update()
//    {
//        if (MenuManager.instance.isPaused || Time.timeScale <= 0f)
//        {
//            return;
//        }
//        if (SceneManager.instance.gameStarted)
//        {
//            bool flag = false;
//            bool flag2 = false;
//            //Touch[] touches = Input.touches;
//            //for (int i = 0; i < touches.Length; i++)
//            //{
//            //    Touch touch = touches[i];
//            //    if (touch.phase == TouchPhase.Began)
//            //    {
//            if (Input.GetMouseButtonDown(0))
//            {
//                //Vector2 position = touch.position;
//                Vector2 position = Input.mousePosition;
//                if ((double)position.x > (double)this.width / 1.3 && (double)position.y > (double)this.height / 1.3)
//                {
//                    if (!MenuManager.instance.isPaused)
//                    {
//                        MenuManager.instance.Pause();
//                    }
//                    return;
//                }
//                if (!SceneManager.instance.inputStarted)
//                {
//                    return;
//                }
//                if (position.x > this.width / 2f)
//                {
//                    if (!flag2)
//                    {
//                        flag2 = true;
//                        InputManager.OnAttackPressed(1);
//                    }
//                }
//                else if (!flag)
//                {
//                    flag = true;
//                    InputManager.OnAttackPressed(-1);
//                }
//            }
//            //    }
//            //}
//        }
//    }
//}
// Decompile from assembly: Assembly-CSharp.dll
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void AttackAction(int side);

    public static InputManager instance;

    private float width;

    private float height;



    public static event InputManager.AttackAction OnAttackPressed;

    private void Awake()
    {
        InputManager.instance = this;
        this.width = (float)Screen.width;
        this.height = (float)Screen.height;
    }

    private void Update()
    {
        if (MenuManager.instance.isPaused || Time.timeScale <= 0f)
        {
            return;
        }
        if (SceneManager.instance.gameStarted)
        {
            bool flag = false;
            bool flag2 = false;
            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                Touch touch = touches[i];
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 position = touch.position;
                    if ((double)position.x > (double)this.width / 1.3 && (double)position.y > (double)this.height / 1.3)
                    {
                        if (!MenuManager.instance.isPaused)
                        {
                            MenuManager.instance.Pause();
                        }
                        return;
                    }
                    if (!SceneManager.instance.inputStarted)
                    {
                        return;
                    }
                    if (position.x > this.width / 2f)
                    {
                        if (!flag2)
                        {
                            flag2 = true;
                            InputManager.OnAttackPressed(1);
                        }
                    }
                    else if (!flag)
                    {
                        flag = true;
                        InputManager.OnAttackPressed(-1);
                    }
                }
            }
            if (Application.isEditor)
            {
                if (Input.GetKeyDown("a"))
                {
                    InputManager.OnAttackPressed(-1);
                }
                if (Input.GetKeyDown("d"))
                {
                    InputManager.OnAttackPressed(1);
                }
            }
        }
    }
}
