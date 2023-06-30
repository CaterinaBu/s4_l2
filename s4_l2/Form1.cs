using System.Drawing.Printing;
using System.Windows.Forms;

namespace s4_l2
{
    public partial class Form1 : Form
    {
        private List<Bedsheet> Gosts { get; set; }
        private List<Cheese> Pacmans { get; set; }
        private Point start; //Стартовая точка для всех объектов
        private readonly int step = 10; //Шаг перемещения
        private readonly int delay = 1000; //Как часто обновляются координаты, мс
        private readonly Random rand = new(); //Если каждый раз вызывать Random().nextInt(),
                                            //то будем получать одинаковые значения,
                                            //так как исходное "зерно" (seed) будет одинаковым.
                                            //Поэтому в отдельную переменную выносим,
                                            //чтобы получать рандомные значения 
        private Thread threadGosts;
        private Thread threadPacmans;
        private bool started = true; //начал ли выполняться поток(нужен для паузы)
        private static readonly Object o = new(); //то, чем будем лочить поток

        public Form1()
        {
            InitializeComponent();
            start = new Point(this.Width / 4, this.Height / 2);

            Gosts = new List<Bedsheet>();
            Pacmans = new List<Cheese>();

            int size = rand.Next(5, 10);
            for (int i = 0; i < size; i++)
            {
                Gosts.Add(new Bedsheet("Призрак", i, 1, start.X, start.Y, 
                    rand.Next(0, this.Width / 4), 
                    rand.Next(0, this.Height / 2)));
            }

            size = rand.Next(5, 10);
            for (int i = 0; i < size; i++)
            {
                Pacmans.Add(new Cheese("Пакман", i+10, 2, start.X, start.Y, 
                    rand.Next(this.Width / 4, this.Width / 2), 
                    rand.Next(this.Height / 2, this.Height)));
            }

        }

        private int TravelTime(Point start, Point end)
        {
            int time;
            double dis = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
            time = (int)dis / step;
            return time;
        }

        private void Thread1()
        {
            try
            {
                Monitor.Enter(o);
                Monitor.PulseAll(o);

                this.Invoke(new Action(() =>
                {
                    label5.Text = "Привидения";
                }));

                foreach (Bedsheet gost in Gosts)
                {
                    Point point = new(gost.x, gost.y);
                    Point end=new(gost.endX, gost.endY);
                    int time = TravelTime(start, end);
                    point.Offset((start.X-end.X)/time, (start.Y - end.Y) / time);
                    gost.x = point.X;
                    gost.y = point.Y;
                }
            }
            catch (Exception ex) { }
            finally
            {
                Monitor.Exit(o);
                this.Invalidate();
                Thread.Sleep(delay);
                Thread1();
            }

        }

        private void Thread2()
        {
            try
            {
                Monitor.Enter(o);
                Monitor.PulseAll(o);

                this.Invoke(new Action(() =>
                {
                    label5.Text = "Пакманы";
                }));

                foreach (Cheese pacman in Pacmans)
                {
                    Point point = new(pacman.x, pacman.y);
                    Point end = new(pacman.endX, pacman.endY);
                    int time = TravelTime(start, end);
                    point.Offset((start.X - end.X) / time, (start.Y - end.Y) / time);
                    pacman.x = point.X;
                    pacman.y = point.Y;
                }
            }
            catch (Exception ex) { }
            finally
            {
                Monitor.Exit(o);
                this.Invalidate();
                Thread.Sleep(delay);
                Thread2();
            }

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.Black), this.Width / 2, 0, this.Width / 2, this.Height);

            foreach (Bedsheet gost in Gosts)
            {
                e.Graphics.DrawImage(Image.FromFile(gost.icon), gost.x, gost.y);
            };

            foreach (Cheese pacman in Pacmans)
            {
                e.Graphics.DrawImage(Image.FromFile(pacman.icon), pacman.x, pacman.y);
            };

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Thread pauseThread = new(new ThreadStart(Pause));
            pauseThread.Start();
        }
        private void Pause()
        {
            started = false;
            Monitor.Enter(o);
            Monitor.PulseAll(o);
            while (!started) { }
            Monitor.Exit(o);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            started = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label5.Text = "отсутствует";
            threadGosts = new Thread(new ThreadStart(Thread1));
            threadPacmans = new Thread(new ThreadStart(Thread2));
            threadGosts.Start();
            threadPacmans.Start();
        }
    }
}