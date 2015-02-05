using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Threading;


namespace Tetris
{
    public partial class MainWindow : Window
    {
        #region Fields
        private Rules rules;
        private Music music;
        private BlockTemplate blockTemplate;
        private readonly DispatcherTimer timer;
        private Dictionary<int, ImageBrush> brushDictionary;
        private Dictionary<Points, int> bottomLocations;
        private Dictionary<int, string> blockDictionary;
        private Label[,] labels;
        private Points[] currentBlock;
        private Points[] baseBlock;
        private int currentBlockReference;
        private bool top;
        private int speed;
        private bool dontRunEventHandler;
        #endregion

        #region Game Setup
        public MainWindow()
        {
            InitializeComponent();
            Setup();

            timer = new DispatcherTimer();
            timer.Interval = new System.TimeSpan(0, 0, 0, 0, speed);
            timer.Tick += timer_Tick;
            timer.IsEnabled = true;
            timer.Start();
        }

        private void Setup()
        {
            dontRunEventHandler = false;
            speed = 500;
            music = new Music(@"Music\TetrisSong.wav");
            rules = new Rules();
            blockTemplate = new BlockTemplate();

            top = false;

            labels = new Label[myGrid.RowDefinitions.Count, myGrid.ColumnDefinitions.Count];
            PopulateGrid();
            PopulateDictionary();
            bottomLocations = new Dictionary<Points, int>();
            InsertNextBlock();
        }

        private void PopulateDictionary()
        {
            blockDictionary = new Dictionary<int, string>();
            blockDictionary[0] = "T";
            blockDictionary[1] = "Line";
            blockDictionary[2] = "Square";
            blockDictionary[3] = "RightZ";
            blockDictionary[4] = "LeftZ";
            blockDictionary[5] = "RightL";
            blockDictionary[6] = "LeftL";

            brushDictionary = new Dictionary<int, ImageBrush>();
            brushDictionary[0] = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/PurpleBlock.png")));
            brushDictionary[1] = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/BlueBlock.png")));
            brushDictionary[2] = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/YellowBlock.png")));
            brushDictionary[3] = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/RedBlock.png")));
            brushDictionary[4] = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/RedBlock.png")));
            brushDictionary[5] = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GreenBlock.png")));
            brushDictionary[6] = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GreenBlock.png")));
            brushDictionary[7] = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/TransparentBlock.png")));

        }

        /*
         * Fills the xaml grid with labels with transparent background
         */
        private void PopulateGrid()
        {
            for (int row = 0; row < 20; row++)
                for (int col = 0; col < 10; col++)
                {
                    Label block = new Label();
                    labels[row, col] = block;
                    Grid.SetRow(labels[row, col], row);
                    Grid.SetColumn(labels[row, col], col);
                    myGrid.Children.Add(labels[row, col]);
                }
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            CheckGame();
        }
        #endregion

        #region add/Clear Blocks
        /*
         * Changes background on labels given Points[]
         */
        private void AddBlock(Points[] gridLocations, Brush brush)
        {
            foreach (Points p in gridLocations)
            {
                if(!rules.MoveCheck(p.ROW, p.COL))
                    labels[p.ROW, p.COL].Background = brush;
            }
        }

        /*
        * sets background of Points[] to transparent background
        */
        private void ClearBlock(Points[] clearLocation)
        {
            Brush brush = brushDictionary[7];
            foreach (Points p in clearLocation)
            {
                if (!rules.MoveCheck(p.ROW, p.COL))
                    labels[p.ROW, p.COL].Background = brush;
            }
        }

        /*
        * inserts random block
        */
        private void InsertNextBlock()
        {
            RandomBlock();
            currentBlock = blockTemplate.GetBlock(blockDictionary[currentBlockReference]);
            baseBlock = blockTemplate.GetBlock(blockDictionary[currentBlockReference]);
            for (int i = 0; i < currentBlock.Length; i++)
            {
                currentBlock[i].COL = (myGrid.ColumnDefinitions.Count / 2) + currentBlock[i].COL;
            }
            AddBlock(currentBlock, brushDictionary[currentBlockReference]);
        }

        /*
        * gets random block from BlockTemplate class
        */
        private void RandomBlock()
        {
            Random rand = new Random();
            currentBlockReference = rand.Next(0, 7);
        }

        /*
        * shifts rows down and changes score and level
        */
        private void ClearRow(List<int> rows)
        {
            dontRunEventHandler = true;
            foreach (int el in rows)
            {
                scoreLabel.Content = "Score: " + rules.score;
                if(rules.nextLevel)
                {
                    levelLabel.Content = "Level: " + rules.level;
                    if(speed > 50)
                    {
                        rules.nextLevel = false;
                        speed -= 50;
                        timer.Interval = new System.TimeSpan(0, 0, 0, 0, speed);
                    }
                }
                for (int row = el; row > 0; row--)
                {

                    for (int col = 0; col < 10; col++)
                    {
                        labels[row, col].Background = labels[row - 1, col].Background;
                    }
                }
            }
            dontRunEventHandler = false;
        }
        #endregion

        #region Gui Btn Labels
        private void GameOver()
        {
            dontRunEventHandler = true;
            gameOverImage.Visibility = System.Windows.Visibility.Visible;
            timer.Stop();
        }

        private void Retry_Btn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region Block Movement
        private void ButtonPressed(object sender, KeyEventArgs e)
        {
            if (dontRunEventHandler) return;
            switch (e.Key)
            {
                case Key.Left:
                    dontRunEventHandler = true;
                    timer.Stop();
                    MoveBlockHorizontal(false);
                    timer.Start();
                    dontRunEventHandler = false;
                    break;

                case Key.Right:
                    dontRunEventHandler = true;
                    timer.Stop();
                    MoveBlockHorizontal(true);
                    timer.Start();
                    dontRunEventHandler = false;
                    break;

                case Key.Down:
                    dontRunEventHandler = true;
                    MoveBlockDown();
                    dontRunEventHandler = false;
                    break;

                case Key.Up:
                    dontRunEventHandler = true;
                    timer.Stop();
                    RotateClockwise();
                    timer.Start();
                    dontRunEventHandler = false;
                    break;

                default:
                    break;
            }
        }

        private void MoveBlockDown()
        {
            dontRunEventHandler = true;
            if (!BottomCheck())
            {
                ClearBlock(currentBlock);

                Points[] tempPoint = currentBlock;
                int count = 0;

                foreach (Points p in currentBlock)
                {
                    tempPoint[count++].ROW = p.ROW + 1;
                }

                currentBlock = tempPoint;
                AddBlock(currentBlock, brushDictionary[currentBlockReference]);
            }
            dontRunEventHandler = false;
        }

        private void MoveBlockHorizontal(bool right)
        {
            dontRunEventHandler = true;
            int shift = right ? 1 : -1;
            if (HorizontalCheck(shift))
            {
                ClearBlock(currentBlock);

                Points[] tempPoint = currentBlock;
                int count = 0;

                foreach (Points p in currentBlock)
                {
                    tempPoint[count++].COL = p.COL + shift;
                }
                currentBlock = tempPoint;
                AddBlock(currentBlock, brushDictionary[currentBlockReference]);
            }
            dontRunEventHandler = false;
        }

        private void RotateClockwise()
        {
            dontRunEventHandler = true;
            ClearBlock(currentBlock);
            Points[] tempBlock = baseBlock;

            for (int i = 0; i < baseBlock.Length; i++)
            {
                int row = (int)baseBlock[i].ROW;
                tempBlock[i].ROW = baseBlock[i].COL;
                tempBlock[i].COL = -1 * row;
            }
            
            if(RotateCheck(tempBlock))
            {
                baseBlock = tempBlock;
                for (int i = 0; i < baseBlock.Length; i++)
                {
                    currentBlock[i].ROW = currentBlock[0].ROW + baseBlock[i].ROW;
                    currentBlock[i].COL = currentBlock[0].COL + baseBlock[i].COL;
                }
            }
            AddBlock(currentBlock, brushDictionary[currentBlockReference]);
            dontRunEventHandler = false;
        }
        #endregion

        #region Game Rule Checks
        /*
         * Checks if block can move down and inserts next block
         */
        private void CheckGame()
        {
            dontRunEventHandler = true;
            if (BottomCheck())
            {
                if (top)
                {
                    GameOver();
                }
                else
                {
                    timer.Stop();
                    rules.AddToBoard(currentBlock);
                    ClearRow(rules.RowCheck());
                    timer.Start();
                    InsertNextBlock();
                }
            }
            else
            {
                MoveBlockDown();
            }
            dontRunEventHandler = false;
        }

        /*
        * Checks if future rotate move is possible
        */
        private bool RotateCheck(Points[] points)
        {
            bool rotatePossible = true;

            foreach (Points el in points)
            {
                if (rules.MoveCheck(currentBlock[0].ROW + el.ROW, currentBlock[0].COL + el.COL))
                    rotatePossible = false;
            }

            return rotatePossible;
        }

        /*
        * Checks if horizontal move is possible
        */
        private bool HorizontalCheck(int direction)
        {
            foreach (Points p in currentBlock)
            {
                if (rules.MoveCheck((int)p.ROW, (int)p.COL + direction))
                    return false;
            }
            return true;
        }

        /*
        * checks if bottom move is possible
        */
        private bool BottomCheck()
        {
            bottomLocations.Clear();
            foreach (Points p in currentBlock)
            {
                bottomLocations[p] = rules.BottomCheck(p);
            }

            foreach (Points p in currentBlock)
            {
                int temp = rules.BottomCheck(p);
                if (((int)p.ROW) == temp)
                {
                    if (temp == 0)
                        top = true;

                    return true;
                }
            }
            return false;
        }
        #endregion

    }
}
