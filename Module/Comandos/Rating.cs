using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Chess_Bot.Module.Comandos
{
    public class Rating : ModuleBase<SocketCommandContext>
    {
        Program p = new Program();

        string player;

        string ratingRapidas, ratingBlitz, ratingBullet;

        WebDriver driver;

        [Command("r")]
        [Alias("player")]
        public async Task MyRating(string player = null)
        {
            this.player = player;
            GetRating();
            await ReplyAsync("Rápidas: " + ratingRapidas + "\nBlitz: " + ratingBlitz + "\nBullet: " + ratingBullet);
        }
        public async Task GetRating()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl("https://www.chess.com/stats/overview/" + player);

            //Espera o carregamento dos elesmentos da página
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            
            ratingRapidas = driver.FindElement(By.CssSelector("#vue-instance > section > a:nth-child(5) > span.footer-block-title")).Text;
            ratingBlitz = driver.FindElement(By.CssSelector("#vue-instance > section > a:nth-child(3) > span.footer-block-title")).Text;
            ratingBullet = driver.FindElement(By.CssSelector("#vue-instance > section > a:nth-child(4) > span.footer-block-title")).Text;
            driver.Close();
        }
    }
}
