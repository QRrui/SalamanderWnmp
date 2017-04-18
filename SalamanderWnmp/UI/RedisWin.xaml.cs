﻿using SalamanderWnmp.Tool;
using StackExchange.Redis;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using static SalamanderWnmp.Tool.RedisHelper;

namespace SalamanderWnmp.UI
{
    /// <summary>
    /// RedisWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RedisWin : Window
    {

        private static ConnectionMultiplexer redisConn = null;
        private ObservableCollection<Node> nodes = null;
        private IServer server = null;

        public RedisWin()
        {
            InitializeComponent();
            if(Common.ConnConfigList == null)
            {
                Common.ConnConfigList = RedisHelper.GetConnList();
            }
            this.nodes = RedisHelper.BuildNodes(Common.ConnConfigList);
            this.tvConn.ItemsSource = this.nodes;
        }

        private void title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
            e.Handled = true;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            e.Handled = true;
        }

        private void btnAddConnect_Click(object sender, RoutedEventArgs e)
        {
            AddRedisConnWin win = new AddRedisConnWin();
            win.Owner = this;
            win.ShowType = 0;
            win.Closing += AddRedisConnWin_Closing;
            win.ShowDialog();
            e.Handled = true;
        }

        private void AddRedisConnWin_Closing(object sender, CancelEventArgs e)
        {
            AddRedisConnWin win = sender as AddRedisConnWin;
            if(win.Tag != null && win.Tag.GetType().Name == "RedisConnConfig")
            {
                RedisConnConfig config = win.Tag as RedisConnConfig;
                if(win.ShowType == 0)
                {
                    Common.ConnConfigList.Add(config.ConnName, config);
                    nodes.Add(new Node { Name = config.ConnName, NodeType = NodeType.Connnection });
                }
                else
                {
                    Common.ConnConfigList[config.ConnName] = config;
                    (tvConn.SelectedItem as Node).Name = config.ConnName;
                }
                RedisHelper.WriteConnList(Common.ConnConfigList);
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            //RedisKey key = (RedisKey)lbKeys.SelectedItem;
            //IDatabase db = redisConn.GetDatabase();
        }
    }

    
}
