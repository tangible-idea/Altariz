using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Generic;

/**
 * 
 *                   var point = se.TransformToVisual(MainContentWrapper).TransformPoint(new Point(0, 0));
 */
namespace ManipulationModeDemo
{
   

    public partial class MainWindow : Window
    {
        #region Instance Definition
            ManipulationModes currentMode = ManipulationModes.All;
            Point m_ptMouse = new Point();
        #endregion 

        #region public MainWindow
        public MainWindow()
        {
            InitializeComponent();

            currentMode = ManipulationModes.Scale | ManipulationModes.Translate;

            //this.MouseMove += new MouseEventHandler(MyPage_MouseMove);
        }


        #endregion 



        #region protected override void OnManipulationStarting
        protected override void OnManipulationStarting(ManipulationStartingEventArgs args)
        {
            args.ManipulationContainer = this;
            args.Mode = currentMode;

            // Adjust Z-order
            //FrameworkElement element = args.Source as FrameworkElement;
            //Panel pnl = element.Parent as Panel;

            //for (int i = 0; i < pnl.Children.Count; i++)
            //    Panel.SetZIndex(pnl.Children[i],
            //        pnl.Children[i] == element ? pnl.Children.Count : i);

            args.Handled = true;
            base.OnManipulationStarting(args);
        }
        #endregion 

        #region protected override void OnManipulationDelta
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs args)
        {
            popup_image_food1.Visibility = Visibility.Hidden;    // 맵 트랜스폼 델타 시에는 팝업을 닫는다.
            popup_image_food2.Visibility = Visibility.Hidden;    // 맵 트랜스폼 델타 시에는 팝업을 닫는다.
            popup_image_food3.Visibility = Visibility.Hidden;    // 맵 트랜스폼 델타 시에는 팝업을 닫는다.
            popup_image_food4.Visibility = Visibility.Hidden;    // 맵 트랜스폼 델타 시에는 팝업을 닫는다.
            

            UIElement element = args.Source as UIElement;
            MatrixTransform xform = element.RenderTransform as MatrixTransform;
            Matrix matrix = xform.Matrix;
            ManipulationDelta delta = args.DeltaManipulation;
            Point center = args.ManipulationOrigin;
            /*
            matrix.Translate(-center.X, -center.Y);
            matrix.Scale(delta.Scale.X, delta.Scale.Y);
            matrix.Rotate(delta.Rotation);
            matrix.Translate(center.X, center.Y);
            matrix.Translate(delta.Translation.X, delta.Translation.Y);           
            xform.Matrix = matrix;
            */
            Matrix to = matrix;
            to.Translate(-center.X, -center.Y);
            to.Scale(delta.Scale.X, delta.Scale.Y);
            to.Rotate(delta.Rotation);
            to.Translate(center.X, center.Y);
            to.Translate(delta.Translation.X, delta.Translation.Y);

            MatrixAnimation b = new MatrixAnimation()
            {
                From = matrix,
                To = to,
                Duration = TimeSpan.FromMilliseconds(0),
                FillBehavior = FillBehavior.HoldEnd
            };
            (element.RenderTransform as MatrixTransform).BeginAnimation(MatrixTransform.MatrixProperty, b);



            tbTranslate.Text = string.Format("Translation: {0}, {1}", delta.Translation.X, delta.Translation.Y);
            tbTranslate.Text += string.Format("\r\nTotal Translation: {0}, {1}", args.CumulativeManipulation.Translation.X, args.CumulativeManipulation.Translation.Y);
          
            args.Handled = true;
            base.OnManipulationDelta(args);
        }
        #endregion 

        #region protected override void OnManipulationCompleted
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
          
          tbCompleted.Text = string.Format("{0}", e.FinalVelocities.LinearVelocity);
          tbCompleted.Text += string.Format("\r\n{0}", e.TotalManipulation.Translation);
          //UIElement el = e.Source as UIElement;
          //el.Effect = new BlurEffect() { Radius= 10.0};

          //MatrixTransform xform = el.RenderTransform as MatrixTransform;
          //Matrix matrix = xform.Matrix;
          //Matrix from = matrix;
          //Matrix to = matrix;
          //to.Translate(e.TotalManipulation.Translation.X * Math.Abs(e.FinalVelocities.LinearVelocity.X), e.TotalManipulation.Translation.Y * Math.Abs(e.FinalVelocities.LinearVelocity.Y));

          //if (Math.Abs(e.FinalVelocities.LinearVelocity.X) > 0.5 || Math.Abs(e.FinalVelocities.LinearVelocity.Y) > 0.5)
          //{
          //  MatrixAnimation b = new MatrixAnimation()
          //  {
          //    From = from,
          //    To = to,
          //    Duration = TimeSpan.FromMilliseconds(500),
          //    FillBehavior = FillBehavior.HoldEnd
          //  };
          //  (el.RenderTransform as MatrixTransform).BeginAnimation(MatrixTransform.MatrixProperty, b);
          //}

          
          base.OnManipulationCompleted(e);
        }
        #endregion 

        #region private void Grid_Loaded
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Play_StoryBoard("hide_rail");
            Play_StoryBoard("fadeout");
            Play_StoryBoard("hide");            

            Play_StoryBoard("moveinit");

            grid_group_spot.Visibility = Visibility.Visible;
            grid_group_food.Visibility = Visibility.Hidden;

            // 이미지 리스트 초기화
            //lstImage_Food.Add(new Image());
            //lstImage_Food.Add(new Image());
            //lstImage_Food.Add(new Image());
            //lstImage_Food.Add(new Image());

            //int nCount = 0;
            //foreach (Image img in lstImage_Food)
            //{
            //    img.Name = "img_popup_food_" + (++nCount);
            //    img.Width = 500;
            //    img.Height = 274;
            //    img.HorizontalAlignment = HorizontalAlignment.Left;
            //    img.VerticalAlignment = VerticalAlignment.Top;
            //}
        }
        #endregion 

        #region private void player1_MouseUp
        private void player1_MouseUp(object sender, MouseButtonEventArgs e)
        {
          if (sender.GetType().ToString() == "System.Windows.Controls.MediaElement")
          {
            MediaElement me = sender as MediaElement;
            MessageBox.Show("11");
          }
        }
        #endregion 

        #region private void player1_StylusSystemGesture
        private void player1_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
        {
          if (e.SystemGesture == SystemGesture.Tap)
          {
          }
        }
        #endregion 

        private void Play_StoryBoard(string strKey)
        {
            var stybd = this.Resources[strKey] as Storyboard;
            if (stybd != null)
                stybd.Begin();
        }


         
        // 중앙 팝업 방식으로 해당 그림 띄움.
        private void SetPopupURI_Spot(String strPath, String strName)
        {
            var strURI = "";
            
            try
            {
                strURI = Path.Combine(Environment.CurrentDirectory, strPath, strName);
                var uri = new Uri(strURI);
                popup_image_spot.Source = new BitmapImage(uri);

                TouchContentMethod();
            }
            catch(Exception e)
            {
                MessageBox.Show("Cannot find current Image.\n["+strURI+" ]");
            }            
        }

        // 중앙 팝업 방식으로 해당 그림 띄움.
        private void SetPopupURI_Rail(String strPath, String strName)
        {
            var strURI = "";

            try
            {
                strURI = Path.Combine(Environment.CurrentDirectory, strPath, strName);
                var uri = new Uri(strURI);
                popup_image_rail.Source = new BitmapImage(uri);

                TouchContentMethod_Rail();
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot find current Image.\n[" + strURI + " ]");
            }
        }

        // 우측 상단 팝업 방식으로 해당 그림 띄움.
        private void SetPopupURI_Food(String strPath, List<String> lstName, Image selectedIcon)
        {
            var strURI = "";

            popup_image_food1.Visibility = Visibility.Hidden;
            popup_image_food2.Visibility = Visibility.Hidden;
            popup_image_food3.Visibility = Visibility.Hidden;
            popup_image_food4.Visibility = Visibility.Hidden;

            //foreach(String strName in lstName)
            for (int i = 0; i<lstName.Count; ++i )
            {
                Image popup_curr = new Image();
                switch(i)
                {
                    case 0:
                    popup_curr = popup_image_food1;
                    break;
                    case 1:
                    popup_curr = popup_image_food2;
                    break;
                    case 2:
                    popup_curr = popup_image_food3;
                    break;
                    case 3:
                    popup_curr = popup_image_food4;
                    break;
                }

                try
                {
                    strURI = Path.Combine(Environment.CurrentDirectory, strPath, lstName[i]);
                    var uri = new Uri(strURI);
                    popup_curr.Source = new BitmapImage(uri);
                    popup_curr.Visibility = Visibility.Visible;                    

                    //lstImage_Food[i].TranslatePoint()
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot find current Image.\n[" + strURI + " ]");
                }

                try
                {

                    //int nX = (int)selectedIcon.Margin.Left;
                    //int nY = (int)selectedIcon.Margin.Top;
                    int nX = (int)m_ptMouse.X;
                    int nY = (int)m_ptMouse.Y - (int)popup_curr.Height;
                    if ((lstName.Count == 3) || (lstName.Count == 4))
                    {
                        nX = 400;
                        nY = -33;
                    }

                    Point nStart= new Point(nX, nY + (i * 255)-15);
                    Point nEnd= new Point(nX, nY + (i * 255));


                    if (nX+500 > root_window.Width) // 오른쪽으로 벗어나면
                    {
                        int nComposeX = nX + 520 - (int)root_window.Width;
                        nStart.X -= nComposeX;
                        nEnd.X -= nComposeX;
                    }
                    if (nY < 0) // 오른쪽으로 벗어나면
                    {
                        int nComposeY = nY + (int)root_window.Height;
                        nStart.Y -= nY;
                        nEnd.Y -= nY;
                    }


                    ControlTranspercyAnimation(popup_curr, 0, 1);
                    ControlTranslateAnimaion(popup_curr, 0f, nStart, nEnd);


                    //ControlTranslateAnimaion(grid_images, 0f, new Point(imgMap.RenderTransformOrigin.X, imgMap.RenderTransformOrigin.Y), new Point(nX-150, nY-500));
                }
                catch (Exception e)
                {
                    MessageBox.Show("food popup animation begin error.\n" + e.ToString());
                }
            }

            
            
        }


        // 여기부터 맛집 [1/6/2014 Mark]
        private void onTouchFood1(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD01.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood2(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD02.png");
            listName.Add("FOOD03.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood3(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD04.png");
            listName.Add("FOOD05.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood4(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD06.png");
            listName.Add("FOOD07.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood5(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD08.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood6(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD09.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood7(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD10.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood8(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD11.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood9(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD12.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood10(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD13.png");
            listName.Add("FOOD14.png");
            listName.Add("FOOD15.png");
            listName.Add("FOOD16.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood11(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD17.png");
            listName.Add("FOOD18.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood12(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD19.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood13(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD20.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood14(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD21.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood15(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD22.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood16(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD23.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood17(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD24.png");
            listName.Add("FOOD25.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood18(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD26.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood19(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD27.png");
            listName.Add("FOOD28.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood20(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD29.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }
        private void onTouchFood21(object sender, TouchEventArgs e)
        {
            Image imgSelectedIcon = sender as Image;
            List<String> listName = new List<String>();
            listName.Add("FOOD30.png");
            SetPopupURI_Food("popup_food", listName, imgSelectedIcon);
        }


        // 여기부터 관광 [1/6/2014 Mark]
        private void onTouchImage1(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_01.png");
        }

        private void onTouchImage2(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_02.png");
        }
        private void onTouchImage3(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_03.png");
        }
        private void onTouchImage4(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_04.png");
        }
        private void onTouchImage5(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_05.png");
        }
        private void onTouchImage6(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_06.png");
        }
        private void onTouchImage7(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_07.png");
        }
        private void onTouchImage8(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_08.png");
        }
        private void onTouchImage9(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_09.png");
        }
        private void onTouchImage10(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_10.png");
        }
        private void onTouchImage11(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_11.png");
        }
        private void onTouchImage12(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_12.png");
        }
        private void onTouchImage13(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_13.png");
        }
        private void onTouchImage14(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_14.png");

        }
        private void onTouchImage15(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_15.png");
        }
        private void onTouchImage16(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_16.png");
        }
        private void onTouchImage17(object sender, TouchEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_16.png");
        }

        private void onClickImage1(object sender, MouseButtonEventArgs e)
        {
            SetPopupURI_Spot("popup_spot", "T_01.png");
        }




        // 바깥 터치시 종료 애니메이션 [12/24/2013 Mark]
        private void onTouchFadeRect(object sender, TouchEventArgs e)
        {
            Play_StoryBoard("hide");
            Play_StoryBoard("hide_rail");

            Play_StoryBoard("fadeout");
        }
        private void onClickMouseFadeRect(object sender, MouseButtonEventArgs e)
        {
            Play_StoryBoard("hide");
            Play_StoryBoard("hide_rail");

            Play_StoryBoard("fadeout");
        }


        // 터치하면 이미지 애니메이션으로 띄움. [12/24/2013 Mark]
        private void TouchContentMethod()
        {
            popup_image_spot.Visibility = Visibility.Visible;
            rct_fadeout.Visibility = Visibility.Visible;

            Play_StoryBoard("fadein");
            Play_StoryBoard("show");
        }

        private void TouchContentMethod_Rail()
        {
            popup_image_rail.Visibility = Visibility.Visible;
            rct_fadeout.Visibility = Visibility.Visible;

            Play_StoryBoard("fadein");
            Play_StoryBoard("show_rail");
        }




        private void StoryInitCompleted(object sender, EventArgs e)
        {
            //rct_fadeout.Visibility = Visibility.Hidden;
        }

        private void StoryHideCompleted(object sender, EventArgs e)
        {
            rct_fadeout.Visibility = Visibility.Hidden;
            popup_image_spot.Visibility = Visibility.Hidden;
        }

        private void RailStoryHideCompleted(object sender, EventArgs e)
        {
            rct_fadeout.Visibility = Visibility.Hidden;
            popup_image_rail.Visibility = Visibility.Hidden;
        }

        

        private void Image_TouchDown(object sender, TouchEventArgs e)
        {

        }


        // 카테고리 클릭 부분.
        private void onTouchCategory1(object sender, TouchEventArgs e)
        {
            grid_group_spot.Visibility = Visibility.Visible;
            grid_group_food.Visibility = Visibility.Hidden;

            popup_image_food1.Visibility = Visibility.Hidden;
            popup_image_food2.Visibility = Visibility.Hidden;
            popup_image_food3.Visibility = Visibility.Hidden;
            popup_image_food4.Visibility = Visibility.Hidden;
        }

        private void onTouchCategory2(object sender, TouchEventArgs e)
        {
            grid_group_spot.Visibility = Visibility.Hidden;
            grid_group_food.Visibility = Visibility.Hidden;
            SetPopupURI_Rail("popup_rail", "rail_info.png");

            popup_image_food1.Visibility = Visibility.Hidden;
            popup_image_food2.Visibility = Visibility.Hidden;
            popup_image_food3.Visibility = Visibility.Hidden;
            popup_image_food4.Visibility = Visibility.Hidden;
        }

        private void onTouchCategory3(object sender, TouchEventArgs e)
        {
            grid_group_spot.Visibility = Visibility.Hidden;
            grid_group_food.Visibility = Visibility.Visible;

            popup_image_food1.Visibility = Visibility.Hidden;
            popup_image_food2.Visibility = Visibility.Hidden;
            popup_image_food3.Visibility = Visibility.Hidden;
            popup_image_food4.Visibility = Visibility.Hidden;
        }


        private void onClickCategory1(object sender, MouseButtonEventArgs e)
        {
            grid_group_spot.Visibility = Visibility.Visible;
            grid_group_food.Visibility = Visibility.Hidden;

            popup_image_food1.Visibility = Visibility.Hidden;
            popup_image_food2.Visibility = Visibility.Hidden;
            popup_image_food3.Visibility = Visibility.Hidden;
            popup_image_food4.Visibility = Visibility.Hidden;
        }
        private void onClickCategory2(object sender, MouseButtonEventArgs e)
        {
            grid_group_spot.Visibility = Visibility.Hidden;
            grid_group_food.Visibility = Visibility.Hidden;
            SetPopupURI_Rail("popup_rail", "rail_info.png");

            popup_image_food1.Visibility = Visibility.Hidden;
            popup_image_food2.Visibility = Visibility.Hidden;
            popup_image_food3.Visibility = Visibility.Hidden;
            popup_image_food4.Visibility = Visibility.Hidden;
        }

        private void onClickCategory3(object sender, MouseButtonEventArgs e)
        {
            grid_group_spot.Visibility = Visibility.Hidden;
            grid_group_food.Visibility = Visibility.Visible;

            popup_image_food1.Visibility = Visibility.Hidden;
            popup_image_food2.Visibility = Visibility.Hidden;
            popup_image_food3.Visibility = Visibility.Hidden;
            popup_image_food4.Visibility = Visibility.Hidden;
        }

        private void ControlTranspercyAnimation(Image imgTaget, double lfStartTranspercy, double lfEndTranspercy)
        {
            {
                Storyboard sb = new Storyboard(); // 스토리보드 할당
                DoubleAnimation da = new DoubleAnimation(); // 에니매이션 제작

                da.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500)); // 재생될 시간 0,0,0,1 == 1초
                da.From = lfStartTranspercy; // 변환될 값의 시작값
                da.To = lfEndTranspercy; // 종료값 --> 현제 애니메이션은 객체의 투명도를 조정하는것이므로 0~1 까지 조절
                //da.AutoReverse = true;  // 자동으로 반대로 재생할것인지를 뭇는것!

                //Storyboard.SetTargetName(da, imgTaget.Name); // 애니메이션과 객체를 연결한다
                Storyboard.SetTarget(da, imgTaget); // 애니메이션과 객체를 연결한다

                Storyboard.SetTargetProperty(da, new PropertyPath(Image.OpacityProperty)); // 애니메이션의 변환할 속성을 설정  
                sb.Children.Add(da); // 스토리보드에 에니메이션 추가
                //sb.RepeatBehavior = RepeatBehavior.Forever; // 영원히 반복
                sb.Begin(imgTaget); // 애니메이션 시작명령
            }
        }

        private void ControlTranslateAnimaion(Grid objTarget, double dBeginTime, Point pStart, Point pEnd)
        {
            Storyboard sbReturn = new Storyboard();

            EasingDoubleKeyFrame kf = null;

            DoubleAnimationUsingKeyFrames daX = null;
            DoubleAnimationUsingKeyFrames daY = null;

            daX = new DoubleAnimationUsingKeyFrames();
            daY = new DoubleAnimationUsingKeyFrames();

            //if (objTarget.RenderTransform == null)
                objTarget.RenderTransform = new TranslateTransform();

            Storyboard.SetTarget(daX, objTarget);
            Storyboard.SetTargetProperty(daX, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            sbReturn.Children.Add(daX);
            Storyboard.SetTarget(daY, objTarget);
            Storyboard.SetTargetProperty(daY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            sbReturn.Children.Add(daY);

            sbReturn.BeginTime = TimeSpan.FromSeconds(dBeginTime);


            //1.시작위치로 변경
            kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            kf.Value = pStart.X;
            daX.KeyFrames.Add(kf);
            kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            kf.Value = pStart.Y;
            daY.KeyFrames.Add(kf);

            //2.목적지 지정
            kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5));
            kf.Value = pEnd.X;
            daX.KeyFrames.Add(kf);
            kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5));
            kf.Value = pEnd.Y;
            daY.KeyFrames.Add(kf);

            //애니메이션 시작
            sbReturn.Begin();
        }

        private void ControlTranslateAnimaion(Image imgTaget, double dBeginTime, Point pStart, Point pEnd)
        {
            Storyboard sbReturn = new Storyboard();

            EasingDoubleKeyFrame kf = null;

            DoubleAnimationUsingKeyFrames daX = null;
            DoubleAnimationUsingKeyFrames daY = null;

            daX = new DoubleAnimationUsingKeyFrames();
            daY = new DoubleAnimationUsingKeyFrames();

            imgTaget.RenderTransform = new TranslateTransform();

            Storyboard.SetTarget(daX, imgTaget);
            Storyboard.SetTargetProperty(daX, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            sbReturn.Children.Add(daX);
            Storyboard.SetTarget(daY, imgTaget);
            Storyboard.SetTargetProperty(daY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            sbReturn.Children.Add(daY);

            sbReturn.BeginTime = TimeSpan.FromSeconds(dBeginTime);


            //1.시작위치로 변경
            kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            kf.Value = pStart.X;
            daX.KeyFrames.Add(kf);
            kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            kf.Value = pStart.Y;
            daY.KeyFrames.Add(kf);

            //2.목적지 지정
            kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5));
            kf.Value = pEnd.X;
            daX.KeyFrames.Add(kf);
            kf = new EasingDoubleKeyFrame();
            kf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5));
            kf.Value = pEnd.Y;
            daY.KeyFrames.Add(kf);

            //애니메이션 시작
            sbReturn.Begin();
        }

        private void onMouseMoveInWindow(object sender, MouseEventArgs e)
        {
            m_ptMouse = e.GetPosition(null);
        }




    }

    #region public class MatrixAnimation
    public class MatrixAnimation : MatrixAnimationBase
    {
      public Matrix? From
      {
        set { SetValue(FromProperty, value); }
        get { return (Matrix)GetValue(FromProperty); }
      }

      public static DependencyProperty FromProperty =
          DependencyProperty.Register("From", typeof(Matrix?), typeof(MatrixAnimation),
              new PropertyMetadata(null));

      public Matrix? To
      {
        set { SetValue(ToProperty, value); }
        get { return (Matrix)GetValue(ToProperty); }
      }

      public static DependencyProperty ToProperty =
          DependencyProperty.Register("To", typeof(Matrix?), typeof(MatrixAnimation),
              new PropertyMetadata(null));

      public IEasingFunction EasingFunction
      {
        get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
        set { SetValue(EasingFunctionProperty, value); }
      }

      public static readonly DependencyProperty EasingFunctionProperty =
          DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(MatrixAnimation),
              new UIPropertyMetadata(null));

      public MatrixAnimation()
      {
      }

      public MatrixAnimation(Matrix toValue, Duration duration)
      {
        To = toValue;
        Duration = duration;
      }

      public MatrixAnimation(Matrix toValue, Duration duration, FillBehavior fillBehavior)
      {
        To = toValue;
        Duration = duration;
        FillBehavior = fillBehavior;
      }

      public MatrixAnimation(Matrix fromValue, Matrix toValue, Duration duration)
      {
        From = fromValue;
        To = toValue;
        Duration = duration;
      }

      public MatrixAnimation(Matrix fromValue, Matrix toValue, Duration duration, FillBehavior fillBehavior)
      {
        From = fromValue;
        To = toValue;
        Duration = duration;
        FillBehavior = fillBehavior;
      }

      protected override Freezable CreateInstanceCore()
      {
        return new MatrixAnimation();
      }

      protected override Matrix GetCurrentValueCore(Matrix defaultOriginValue, Matrix defaultDestinationValue, AnimationClock animationClock)
      {
        if (animationClock.CurrentProgress == null)
        {
          return Matrix.Identity;
        }

        var normalizedTime = animationClock.CurrentProgress.Value;
        if (EasingFunction != null)
        {
          normalizedTime = EasingFunction.Ease(normalizedTime);
        }

        var from = From ?? defaultOriginValue;
        var to = To ?? defaultDestinationValue;

        var newMatrix = new Matrix(
                ((to.M11 - from.M11) * normalizedTime) + from.M11,
                ((to.M12 - from.M12) * normalizedTime) + from.M12,
                ((to.M21 - from.M21) * normalizedTime) + from.M21,
                ((to.M22 - from.M22) * normalizedTime) + from.M22,
                ((to.OffsetX - from.OffsetX) * normalizedTime) + from.OffsetX,
                ((to.OffsetY - from.OffsetY) * normalizedTime) + from.OffsetY);

        return newMatrix;
      }
    }
    #endregion 

    #region public class LinearMatrixAnimation 
    public class LinearMatrixAnimation : AnimationTimeline
    {

      public Matrix? From
      {
        set { SetValue(FromProperty, value); }
        get { return (Matrix)GetValue(FromProperty); }
      }
      public static DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(Matrix?), typeof(LinearMatrixAnimation), new PropertyMetadata(null));

      public Matrix? To
      {
        set { SetValue(ToProperty, value); }
        get { return (Matrix)GetValue(ToProperty); }
      }
      public static DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(Matrix?), typeof(LinearMatrixAnimation), new PropertyMetadata(null));

      public LinearMatrixAnimation()
      {
      }

      public LinearMatrixAnimation(Matrix from, Matrix to, Duration duration)
      {
        Duration = duration;
        From = from;
        To = to;
      }

      public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
      {
        if (animationClock.CurrentProgress == null)
        {
          return null;
        }

        double progress = animationClock.CurrentProgress.Value;
        Matrix from = From ?? (Matrix)defaultOriginValue;

        if (To.HasValue)
        {
          Matrix to = To.Value;
          Matrix newMatrix = new Matrix(((to.M11 - from.M11) * progress) + from.M11, 0, 0, ((to.M22 - from.M22) * progress) + from.M22,
                                        ((to.OffsetX - from.OffsetX) * progress) + from.OffsetX, ((to.OffsetY - from.OffsetY) * progress) + from.OffsetY);
          return newMatrix;
        }

        return Matrix.Identity;
      }

      protected override System.Windows.Freezable CreateInstanceCore()
      {
        return new LinearMatrixAnimation();
      }

      public override System.Type TargetPropertyType
      {
        get { return typeof(Matrix); }
      }
    }
    #endregion 
}
