namespace MemoryGame
{
    public class Program
    {
        public static void Main()
        {
            SettingsForm settings = new SettingsForm();

            settings.ShowDialog();
            GameBoardForm gameBoard = new GameBoardForm(settings.BoardSize, settings.FirstPlayerName, settings.SecondPlayerName);

            while (gameBoard.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                gameBoard = new GameBoardForm(settings.BoardSize, settings.FirstPlayerName, settings.SecondPlayerName);
            }
        }
    }
}
