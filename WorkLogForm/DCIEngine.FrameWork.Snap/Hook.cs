using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WorkLogForm.DCIEngine.FrameWork.Snap
{

    public class Hook
    {
        #region Hook类型
        public enum HookTypeEnum
        {
            KeyboardHook = 13,  //底层全局键盘钩子
            MouseHook = 14,   //底层全局鼠标钩子
        }

        #endregion

        public class KeyboardMouseHook
        {
            #region 全局字段

            /// <summary>
            /// Hook类型（KeyboardMouseHook构造时指定）
            /// </summary>
            private int HookType;

            /// <summary>
            /// 钩子句柄
            ///  Hook的SetWindowsHookEx方法的返回值（成功则返回钩子程的入口地址，否则返回null）
            /// </summary>
            private IntPtr hookID = IntPtr.Zero;

            #endregion

            #region Keyboard委托

            /// <summary>
            /// 钩子子程（触发HookEvent时调用）
            /// </summary>
            private delegate IntPtr KeyboardHookEventHandler(int nCode, IntPtr wParam, ref KeyboardHookStruct lParam);

            /// <summary>
            /// 钩子子程的入口地址
            /// </summary>
            private KeyboardHookEventHandler procKeyboard;

            /// <summary>
            /// 用户对HookEvent的处理方法，由KeyboardHookEventHandler回调
            /// 为了实现不同的处理方法（在不同场合调用本DLL），KeyboardHookEventHandler调用UserKeyboardHookEventHandler进行处理
            /// </summary>
            /// <param name="hookStruct"></param>
            public delegate void UserKeyboardHookEventHandler(KeyboardHookStruct hookStruct, out bool isNeedStop);

            /// <summary>
            /// 用户对HookEvent的处理方法的入口地址
            /// 其具体实现不在本DLL中，KeyboardHook构造时传入
            /// </summary>
            private static UserKeyboardHookEventHandler userProcKeyboard;

            #endregion

            #region Mouse委托

            /// <summary>
            /// 钩子子程（触发HookEvent时调用）
            /// </summary>
            private delegate IntPtr MouseHookEventHandler(int nCode, IntPtr wParam, ref MouseHookStruct lParam);

            /// <summary>
            /// 钩子子程的入口地址
            /// </summary>
            private MouseHookEventHandler procMouse;

            /// <summary>
            /// 用户对HookEvent的处理方法，由MouseHookEventHandler回调
            /// 为了实现不同的处理方法（在不同场合调用本DLL），MouseHookEventHandler调用UserMouseHookEventHandler进行处理
            /// </summary>
            /// <param name="hookStruct"></param>
            public delegate void UserMouseHookEventHandler(MouseHookStruct hookStruct, out bool isNeedStop);

            /// <summary>
            /// 用户对HookEvent的处理方法的入口地址
            /// 其具体实现不在本DLL中，KeyboardHook构造时传入
            /// </summary>
            private static UserMouseHookEventHandler userProcMouse;

            #endregion

            #region Hook结构

            //TODO: 可根据需要修改Hook结构成员的可见性
            /// <summary>
            /// Hook结构（Keyboard的HookEvent触发时返回的结构）
            /// </summary>
            public struct KeyboardHookStruct
            {
                public int vkCode; //表示一个在1到254间的虚似键盘码 
                int scanCode;      //表示硬件扫描码     
                int flags;
                int time;
                int dwExtraInfo;
            }

            //TODO: 可根据需要修改Hook结构成员的可见性
            /// <summary>
            /// Hook结构（Keyboard的HookEvent触发时返回的结构）
            /// </summary>
            public struct MouseHookStruct
            {
                public MouseActionEnum mouseAction;
                public Point pt;
                int hWnd;    //鼠标点击的控件的句柄
                int wHitTestCode;
                int dwExtraInfo;

                /// <summary>
                /// 鼠标指针的坐标
                /// </summary>
                public struct Point
                {
                    public int x;
                    public int y;
                }

                /// <summary>
                /// 鼠标动作
                /// </summary>
                public enum MouseActionEnum
                {
                    Move,
                    LeftButtonDown,
                    RightButtonDown,
                    MiddleButtonDown,
                    LeftButtonUp,
                    RightButtonUp,
                    MiddleButtonUp,
                    LeftButtonDoubleClick,
                    RightButtonDoubleClick,
                    MiddleButtonDoubleClick,
                }
            }

            #endregion

            #region 导入DLL的方法


            /// <summary>
            /// 设置钩子(Keyboard)
            /// </summary>
            /// <param name="idHook"></param>
            /// <param name="lpfn"></param>
            /// <param name="hMod"></param>
            /// <param name="dwThreadId"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookEventHandler lpfn, IntPtr hMod, uint dwThreadId);

            /// <summary>
            /// 设置钩子（Mouse）
            /// </summary>
            /// <param name="idHook"></param>
            /// <param name="lpfn"></param>
            /// <param name="hMod"></param>
            /// <param name="dwThreadId"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook, MouseHookEventHandler lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            /// <summary>
            /// 调用钩子链表中的下一个钩子（Keyboard）
            /// </summary>
            /// <param name="hhk"></param>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref KeyboardHookStruct lParam);

            /// <summary>
            /// 调用钩子链表中的下一个钩子（Mouse）
            /// </summary>
            /// <param name="hhk"></param>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref MouseHookStruct lParam);

            [DllImport("kernel32.dll")]
            private static extern int GetCurrentThreadId();

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);

            #endregion

            #region 构造方法

            private KeyboardMouseHook()  //默认构造方法
            {
            }

            public KeyboardMouseHook(HookTypeEnum hookType)
            {
                HookType = (int)hookType;//设置Hook类型
            }

            #endregion

            #region 钩子子程的实现（回调）


            /// <summary>
            /// 钩子之程的实现(keyboard)
            /// </summary>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, ref KeyboardHookStruct lParam)
            {
                if (nCode >= 0)
                {
                    if (userProcKeyboard != null)
                    {
                        bool flag = false;
                        userProcKeyboard(lParam, out flag);
                        if (flag)
                        {
                            return (System.IntPtr)1; //1没有意义。在这里为了返回。
                        }
                    }
                }
                return CallNextHookEx(hookID, nCode, wParam, ref lParam); //调用下一个钩子，使得捕获的消息继续传送
            }

            private IntPtr MouseHookCallback(int nCode, IntPtr wParam, ref MouseHookStruct lParam)
            {
                if (nCode >= 0)
                {
                    if (userProcMouse != null)
                    {
                        // 鼠标消息
                        const int WM_MOUSEMOVE = 0x200;
                        const int WM_LBUTTONDOWN = 0x201;
                        const int WM_RBUTTONDOWN = 0x204;
                        const int WM_MBUTTONDOWN = 0x207;
                        const int WM_LBUTTONUP = 0x202;
                        const int WM_RBUTTONUP = 0x205;
                        const int WM_MBUTTONUP = 0x208;
                        const int WM_LBUTTONDBLCLK = 0x203;
                        const int WM_RBUTTONDBLCLK = 0x206;
                        const int WM_MBUTTONDBLCLK = 0x209;

                        //设置MouseHookStruct.mouseAction
                        MouseHookStruct.MouseActionEnum mAction = new MouseHookStruct.MouseActionEnum();
                        switch ((int)wParam)
                        {
                            case WM_MOUSEMOVE:
                                mAction = MouseHookStruct.MouseActionEnum.Move;
                                break;
                            case WM_LBUTTONDOWN:
                                mAction = MouseHookStruct.MouseActionEnum.LeftButtonDown;
                                break;
                            case WM_LBUTTONUP:
                                mAction = MouseHookStruct.MouseActionEnum.LeftButtonUp;
                                break;
                            case WM_LBUTTONDBLCLK:
                                mAction = MouseHookStruct.MouseActionEnum.LeftButtonDoubleClick;
                                break;
                            case WM_RBUTTONDOWN:
                                mAction = MouseHookStruct.MouseActionEnum.RightButtonDown;
                                break;
                            case WM_RBUTTONUP:
                                mAction = MouseHookStruct.MouseActionEnum.RightButtonUp;
                                break;
                            case WM_RBUTTONDBLCLK:
                                mAction = MouseHookStruct.MouseActionEnum.RightButtonDoubleClick;
                                break;
                            case WM_MBUTTONDOWN:
                                mAction = MouseHookStruct.MouseActionEnum.MiddleButtonDown;
                                break;
                            case WM_MBUTTONUP:
                                mAction = MouseHookStruct.MouseActionEnum.MiddleButtonUp;
                                break;
                            case WM_MBUTTONDBLCLK:
                                mAction = MouseHookStruct.MouseActionEnum.MiddleButtonDoubleClick;
                                break;
                        }
                        lParam.mouseAction = mAction;

                        bool flag = false;
                        userProcMouse(lParam, out flag);
                        if (flag)
                        {
                            return (System.IntPtr)1; //同上
                        }
                    }
                }
                return CallNextHookEx(hookID, nCode, wParam, ref lParam);
            }
            #endregion

            #region 公共方法：设置钩子、释放钩子

            /// <summary>
            /// 重载：设置Keyboard钩子（开始Hook）
            /// </summary>
            public void InstallHook(UserKeyboardHookEventHandler userKeyboardProc)
            {
                userProcKeyboard = userKeyboardProc; //传入UserKeyboardHookEventHandler方法的入口地址
                procKeyboard = new KeyboardHookEventHandler(KeyboardHookCallback); //告知钩子子程的具体实现

                hookID = SetWindowsHookEx(
                    HookType,  // 钩子的类型
                    procKeyboard,  //钩子子程的入口地址，当钩子钩到任何消息后便调用钩子子程
                    GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), //应用程序实例的句柄
                    0); //0表示钩子子程与所有的线程关联（全局钩子）
            }

            /// <summary>
            /// 重载：设置Mouse钩子（开始Hook）
            /// </summary>
            public void InstallHook(UserMouseHookEventHandler userMouseProc)
            {
                userProcMouse = userMouseProc; //传入UserMouseHookEventHandler方法的入口地址
                procMouse = new MouseHookEventHandler(MouseHookCallback); //告知钩子子程的具体实现

                hookID = SetWindowsHookEx(
                    HookType,  // 钩子的类型
                    procMouse,  //钩子子程的入口地址，当钩子钩到任何消息后便调用钩子子程
                    GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), //应用程序实例的句柄
                    0); //0表示钩子子程与所有的线程关联（全局钩子）
            }

            /// <summary>
            /// 释放钩子（Hook结束）
            /// </summary>
            public void UninstallHook()
            {
                UnhookWindowsHookEx(hookID);
            }

            #endregion
        }


    }
}
