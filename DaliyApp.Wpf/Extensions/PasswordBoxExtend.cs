using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DaliyApp.Wpf.Extensions
{
    public class PasswordBoxExtend
    {
        public static string GetPwd(DependencyObject obj)
        {
            return (string)obj.GetValue(PwdProperty);
        }

        public static void SetPwd(DependencyObject obj, string value)
        {
            obj.SetValue(PwdProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PwdProperty =
            DependencyProperty.RegisterAttached("Pwd", typeof(string), typeof(PasswordBoxExtend),
                new PropertyMetadata("", OnChagePwd));

        /// <summary>
        /// 密码改变事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnChagePwd(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordbox = d as PasswordBox;
            string newpwd = (string)e.NewValue;
            if (passwordbox != null && passwordbox.Password != newpwd)
            {
                passwordbox.Password = newpwd;
            }
        }
    }

    /// <summary>
    /// 密码框行为 password改变自定义附加属性跟着改变
    /// </summary>
    public class PaswordBoxBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPasswordChanged;//添加事件
        }

        /// <summary>
        ///改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PasswordBoxExtend.GetPwd(passwordBox);//获取值
            if (passwordBox.Password != password && passwordBox.Password != password)
            {
                PasswordBoxExtend.SetPwd(passwordBox, passwordBox.Password);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnPasswordChanged;//移除事件
        }
    }
}