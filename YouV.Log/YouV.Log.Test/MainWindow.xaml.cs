using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YouV.Log.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Task.Run(() =>
            //{
            //    Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.DEBUG);
            //    Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.INFO);
            //    Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.WARN);
            //    Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.ERROR);
            //    Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.FATAL);

            //});


            //Task.Run(() =>
            //{
            //    Logger.WriteLog("张三已登录", "Authority");
            //    Logger.WriteLog("打开扫码枪失败!", "Scanner", LogLevel.ERROR);
            //    Logger.WriteLog("视觉反馈坐标是{X:1.001,Y:2.652}", "Vision", LogLevel.WARN);
            //    Logger.WriteLog("收到消息[ABS#X#51.075]", "Mqtt", LogLevel.FATAL);
            //});


            Task.Run(() =>
            {
                Logger.WriteLog("I am a msg of moduleA that will written to all.", "A", alsoIntoAll: true);
                Logger.WriteLog("I am a msg of moduleA that won't written to all.", "A", alsoIntoAll: false);
            });
        }
    }
}
