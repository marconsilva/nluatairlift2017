using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Airlift.NLU.Common.Accelerator.ServiceModels;
using Microsoft.Cognitive.LUIS;

namespace Microsoft.Airlift.NLU.Common.Accelerator
{
    public sealed class OutputMessagesManager
    {
        private readonly OutputMessagesManagerLanguage language;

        public OutputMessagesManager(OutputMessagesManagerLanguage language)
        {
            this.language = language;
        }
        public string GetUnknownIntentOutput()
        {
            switch (language)
            {
                case OutputMessagesManagerLanguage.en_US:
                    return "I didn't get what you said.";
                case OutputMessagesManagerLanguage.pt_PT:
                default:
                    return "Não entendi o que disseste.";
            }
        }

        public string GetLightsOutIntentOutput()
        {
            switch (language)
            {
                case OutputMessagesManagerLanguage.en_US:
                    return "I've turned off the light.";
                case OutputMessagesManagerLanguage.pt_PT:
                default:
                    return "Apaguei a Luz.";
            }

        }

        public string GetLightsOnIntentOutput()
        {
            switch (language)
            {
                case OutputMessagesManagerLanguage.en_US:
                    return "I've turned on the light.";
                case OutputMessagesManagerLanguage.pt_PT:
                default:
                    return "Acendi a Luz.";
            }
        }

        public string GetForecastTypeMessage(WeatherType forecast)
        {
            switch (forecast)
            {
                case WeatherType.Cloudy:
                    return language == OutputMessagesManagerLanguage.en_US? "I see some clouds" : "Estão umas nuvens";
                case WeatherType.Sunny:
                    return language == OutputMessagesManagerLanguage.en_US ? "Can't you see the sun outside?" : "Está sol, Não vês?";
                case WeatherType.Rainy:
                    return language == OutputMessagesManagerLanguage.en_US ? "It's raining, don't forget your umbrela" : "Está a chover, leva guarda-chuva";
                case WeatherType.Foggy:
                    return language == OutputMessagesManagerLanguage.en_US ? "It's foggy" : "está nevoeiro";
                case WeatherType.LightRain:
                    return language == OutputMessagesManagerLanguage.en_US ? "It's a bit rainy" : "está a chover, mas não muito";
                case WeatherType.Unknown:
                default:
                    return language == OutputMessagesManagerLanguage.en_US ? "The weather is odd..." : "Está um tempo estranho";
            }
        }

        public string GetForecastMinTemperatureMessage(float minTemperature)
        {
            switch (language)
            {
                case OutputMessagesManagerLanguage.en_US:
                    return $"The lowest temperature will be {minTemperature:0.##} degrees";
                case OutputMessagesManagerLanguage.pt_PT:
                default:
                    return $"A temperatura minima é de {minTemperature:0.##} graus";
            }
        }

        public string GetForecastMaxTemperatureMessage(float maxTemperature)
        {
            switch (language)
            {
                case OutputMessagesManagerLanguage.en_US:
                    return $"The highest temperature will be {maxTemperature:0.##} degrees";
                case OutputMessagesManagerLanguage.pt_PT:
                default:
                    return $"A temperatura minima é de {maxTemperature:0.##} graus";
            }
        }

        public string GetMusicNameNotFoundMessageOutput()
        {
            switch (language)
            {
                case OutputMessagesManagerLanguage.en_US:
                    return "I need to know the name of the song you want me to play...";
                case OutputMessagesManagerLanguage.pt_PT:
                default:
                    return "Tens que me dizer o nome da música que queres tocar...";
            }

        }

        public string GetMusicNotFoundMessageOutput()
        {
            switch (language)
            {
                case OutputMessagesManagerLanguage.en_US:
                    return "I really don't know that song...";
                case OutputMessagesManagerLanguage.pt_PT:
                default:
                    return "Tens que me dizer o nome da música que queres tocar...";
            }
        }

        public string GetAgeValueMissingMessage()
        {
            return language == OutputMessagesManagerLanguage.en_US ? "I didn't get your age, could you repeat?" : "Não entendi qual a sua idade";
        }

        public string GetAgeMessage(string value)
        {
            int age;
            if(!int.TryParse(value.Substring(0, 3), out age))
                return language == OutputMessagesManagerLanguage.en_US ? "I didn't get your age, could you repeat?" : "Não entendi qual a sua idade";

            if(age < 20)
                return language == OutputMessagesManagerLanguage.en_US ? $"Liar, you are not {age} years old" : $"Mentiroso, não tens nada {age} anos";

            return language == OutputMessagesManagerLanguage.en_US ? $"I can tell, {age} years old looks about right" : $"nota-se, {age} anos seria o que eu lhe daria";
        }

        public string GetWhoIsMessage(string key, string value)
        {
            switch (key)
            {
                case "builtin.encyclopedia.people.person":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a person" : $"{value} é uma pessoa";
                case "builtin.encyclopedia.film.producer":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a film producer" : $"{value} é um produtor de filmes";
                case "builtin.encyclopedia.film.cinematographer":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a cinematographer" : $"{value} é um cinematografo";
                case "builtin.encyclopedia.royalty.monarch":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a monarch" : $"{value} é um membro da nobresa";
                case "builtin.encyclopedia.film.director":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a film director" : $"{value} é um realizador de cinema";
                case "builtin.encyclopedia.film.writer":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a film writer" : $"{value} é um escritor de cinema";
                case "builtin.encyclopedia.film.actor":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is an actor" : $"{value} é um actor";
                case "builtin.encyclopedia.martial_arts.martial_artist":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a martial artist" : $"{value} é um praticante de artes marciais";
                case "builtin.encyclopedia.architecture.architect":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is an architect" : $"{value} é um arquiteto";
                case "builtin.encyclopedia.geography.mountaineer":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a mountaineer" : $"{value} é um montanhista";
                case "builtin.encyclopedia.celebrities.celebrity":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a celebrity" : $"{value} é uma celebridade";
                case "builtin.encyclopedia.music.musician":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a musician" : $"{value} é um musico";
                case "builtin.encyclopedia.soccer.player":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a soccer player" : $"{value} é um jogador de futebol";
                case "builtin.encyclopedia.baseball.player":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a baseball player" : $"{value} é um jogador de basebol";
                case "builtin.encyclopedia.basketball.player":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a basketball player" : $"{value} é um jogador de basquetebol";
                case "builtin.encyclopedia.olympics.athlete":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is an olympic athelete" : $"{value} é um ateleta olympico";
                case "builtin.encyclopedia.basketball.coach":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a basketball coach" : $"{value} é um treinador de basquetebol";
                case "builtin.encyclopedia.american_football.coach":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a football coach" : $"{value} é um treinador de futebol americano";
                case "builtin.encyclopedia.cricket.coach":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a cricket coach" : $"{value} é um treinador de cricket";
                case "builtin.encyclopedia.government.us_president":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a United States President" : $"{value} é um presidente dos estados unidos";
                case "builtin.encyclopedia.government.politician":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a politician" : $"{value} é um politico";
                case "builtin.encyclopedia.government.us_vice_president":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a United States Vice President" : $"{value} é um vice presidente americano";
                case "builtin.encyclopedia.ice_hockey.coach":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a ice hockey coach" : $"{value} é um treinador de hockey no gelo";
                case "builtin.encyclopedia.ice_hockey.player":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a ice hockey player" : $"{value} é um jogador de hockey no gelo";
                case "builtin.encyclopedia.book.author":
                    return language == OutputMessagesManagerLanguage.en_US ? $"{value} is a book author" : $"{value} é um autor";
                default:
                    return language == OutputMessagesManagerLanguage.en_US ? $"Don't know who {value} is" : $"não sei quem é {value}";
            }

         
        }
        /*[LuisIntent("whois")]
    public async Task WhoIsIntent(IDialogContext context, LuisResult result)
    {
        foreach (var entity in result.Entities)
        {
            await GetWhoIsMessage(context, entity.Type, entity.Entity);
        }
        context.Wait(MessageReceived);
    }
    
    private async Task GetWhoIsMessage(IDialogContext context, string key, string value)
    {
        switch (key)
        {
            case "builtin.encyclopedia.people.person": 
                await context.PostAsync($"{value} is a person");
                break;
            case "builtin.encyclopedia.film.producer":
                await context.PostAsync($"{value} is a film producer");
                break;
            case "builtin.encyclopedia.film.cinematographer":
                await context.PostAsync($"{value} is a cinematographer");
                break;
            case "builtin.encyclopedia.royalty.monarch":
                await context.PostAsync($"{value} is a monarch");
                break;
            case "builtin.encyclopedia.film.director":
                await context.PostAsync($"{value} is a film director");
                break;
            case "builtin.encyclopedia.film.writer":
                await context.PostAsync($"{value} is a film writer");
                break;
            case "builtin.encyclopedia.film.actor":
                await context.PostAsync($"{value} is an actor");
                break;
            case "builtin.encyclopedia.martial_arts.martial_artist":
                await context.PostAsync($"{value} is a martial artist");
                break;
            case "builtin.encyclopedia.architecture.architect":
                await context.PostAsync($"{value} is an architect");
                break;
            case "builtin.encyclopedia.geography.mountaineer":
                await context.PostAsync($"{value} is a mountaineer");
                break;
            case "builtin.encyclopedia.celebrities.celebrity":
                await context.PostAsync($"{value} is a celebrity");
                break;
            case "builtin.encyclopedia.music.musician":
                await context.PostAsync($"{value} is a musician");
                break;
            case "builtin.encyclopedia.soccer.player":
                await context.PostAsync($"{value} is a soccer player");
                break;
            case "builtin.encyclopedia.baseball.player":
                await context.PostAsync($"{value} is a baseball player");
                break;
            case "builtin.encyclopedia.basketball.player":
                await context.PostAsync($"{value} is a basketball player");
                break;
            case "builtin.encyclopedia.olympics.athlete":
                await context.PostAsync($"{value} is an olympic athelete");
                break;
            case "builtin.encyclopedia.basketball.coach":
                await context.PostAsync($"{value} is a basketball coach");
                break;
            case "builtin.encyclopedia.american_football.coach":
                await context.PostAsync($"{value} is a football coach");
                break;
            case "builtin.encyclopedia.cricket.coach":
                await context.PostAsync($"{value} is a cricket coach");
                break;
            case "builtin.encyclopedia.government.us_president":
                await context.PostAsync($"{value} is a United States President");
                break;
            case "builtin.encyclopedia.government.politician":
                await context.PostAsync($"{value} is a politician");
                break;
            case "builtin.encyclopedia.government.us_vice_president":
                await context.PostAsync($"{value} is a United States Vice President");
                break;
            case "builtin.encyclopedia.ice_hockey.coach":
                await context.PostAsync($"{value} is a ice hockey coach");
                break;
            case "builtin.encyclopedia.ice_hockey.player":
                await context.PostAsync($"{value} is a ice hockey player");
                break;
            case "builtin.encyclopedia.book.author":
                await context.PostAsync($"{value} is a book author");
                break;
            default:
                await context.PostAsync($"Don't know who {value} is");
                break;
        }*/
    }
}
