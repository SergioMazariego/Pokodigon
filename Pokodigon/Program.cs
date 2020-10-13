using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Pokodigon
{
    class Program
    {
        public static string[] Names = { "Pikachu", "Lucario", "Charmander", "Caterpie" , "Ekans", "Mew", "Weedle", 
                                        "Pidgeotto","Vulpix", "Gloom", "Persian"};
        public static string[] Types = { "Electrico", "Pelea", "Fuego", "Bicho", "Veneno", "Psiquico", "Bicho", "Tierra",
                                        "Fuego", "Bicho", "Normal"};
        public static int[] Index = new int[11]; //Arreglo para guardar los indices de pokemons ya utilizados
        public static int[] IndexRival = new int[6]; //Arreglo para saber si un pokemon rival ya peleó
        //Arreglo de tipo Pokemon para guardar los pokemones del equipo rival
        public static Pokemon[] RivalTeam = new Pokemon[6];
        public static Pokemon[] UserTeam = new Pokemon[6];
        public static Pokemon currentRivalPokemon;
        public static Pokemon currentUserPokemon;
        public static int userPokemonIndex = 0; //Indice donde ira el pokemon que atrape el usuario para su equipo
        static void Main(string[] args)
        {
            
            StartGame();
        }

        static void StartGame()
        {
            
            AddRivalTeam(Names, Types);
            currentRivalPokemon = RivalTeam[CallPokemonRival()]; //Aqui se toma el pokemon que va a pelear de entre los pokemons del rival
            currentRivalPokemon.setAttacksDamage();
            currentRivalPokemon.setAttacksNames();
            Console.WriteLine("Tu pokemon es: {0}!", currentUserPokemon.Name);
            Console.Write("Equipo rival: ");
            for (int i = 0; i < 6; i++)
            {
                Console.Write("[{0}] ", RivalTeam[i].Name);
            }
            Console.WriteLine();
            Figth();
        }
        static void AddRivalTeam(string[] names, string[] types)
        {
            for (int i = 0; i < 6; i++)
            {
                int indexPokemon = GetIndex();
                RivalTeam[i] = new Pokemon(names[indexPokemon], types[indexPokemon]);
                RivalTeam[i].setAttacksNames();
                RivalTeam[i].setAttacksDamage();
            }
            currentUserPokemon = new Pokemon(names[GetIndex()], types[GetIndex()]);
            currentUserPokemon.setAttacksNames();
            currentUserPokemon.setAttacksDamage();
            UserTeam[userPokemonIndex] = currentUserPokemon;
            userPokemonIndex++;
    }
        static Random random = new Random(DateTime.Now.Second);
        static int GetIndex()
        {
            
            int IndexPokemon = random.Next(0, 11);           
                int contador = 0;
                while (contador != 6)
                {
                    // verifica que el pokemon este disponible.
                    if (Index[IndexPokemon] != -1)
                    {
                        Index[IndexPokemon] = -1;
                        break;
                    }
                    else
                    {
                        // si la carta no esta disponible, entonces genera otra posicion.
                        IndexPokemon = random.Next(0, 11);
                    }
                    contador++;
                }
                return IndexPokemon;
        }
        static void Figth()
        {
            //El juego continua mientras la funcion de terminado devuelve 0
            while (GameFinish() == 0)
            {
                Console.WriteLine("\n+------------+");
                Console.WriteLine("|    HP: {0}", currentRivalPokemon.HP);
                Console.WriteLine("+------------+");
                Console.WriteLine("Pokemon rival: {0}", currentRivalPokemon.Name);
                Console.WriteLine("\nTu pokemon actual: {0}", currentUserPokemon.Name); ;
                Console.WriteLine("+------------+");
                Console.WriteLine("|    HP: {0}", currentUserPokemon.HP);
                Console.WriteLine("+------------+");

                SelectTurnRandom();
                
                Console.Clear();
            }
            PlayAgain();
        }

        static void SelectTurnRandom()
        {
            
            Random random = new Random();
            //Que pueda devolver 1 o 2
            int turn = random.Next(1, 3);
            if (turn == 1)
            {
                UserTurn();
                
            }
            else
            {
                RivalTurn();

            }

        }
        static void RivalTurn()
        {
            Console.WriteLine("El rival tiene el turno!");
            Random random = new Random();
            //Que pueda devolver 1 o 2
            int attack = random.Next(1, 3);
            RivalTurn(currentRivalPokemon.Name, RivalRandomAttack(attack), RivalRandomDamage(attack), currentUserPokemon.HP);
        }

        static void RivalTurn(string rivalpokemonName, string attackName, int attackDamage, int UserHP)
        {
            Console.WriteLine();
            Console.WriteLine("{0} ha usado {1}!", rivalpokemonName, attackName);
            Console.WriteLine("Haz sufrido un dano de {0} puntos!", attackDamage);
            currentUserPokemon.HP = currentUserPokemon.HP - attackDamage;
            Console.WriteLine("Cargando...");
            Thread.Sleep(5000);
        }

        static string RivalRandomAttack(int attack)
        {
            if (attack == 1)
            {
                return currentRivalPokemon.Attack1;
            } 
            else
            {
                return currentRivalPokemon.Attack2;
            }
        }
        static int RivalRandomDamage(int attack)
        {
            if (attack == 1)
            {
                return currentRivalPokemon.AttackDamage1;
            }
            else
            {
                return currentRivalPokemon.AttackDamage2;
            }
        }
        //Funcion para darle el turno al pokemon actual del usuario
        static void UserTurn()
        {            
                Console.WriteLine("\nTienes el turno!");
                Console.WriteLine("__________________");
                Console.WriteLine("Elige tu ataque:");
                Console.WriteLine("\n1. {0} | Puntos de ataque: {1}\n2. {2} | Puntos de ataque: {3}", currentUserPokemon.Attack1, currentUserPokemon.AttackDamage1, currentUserPokemon.Attack2, currentUserPokemon.AttackDamage2);
                Console.WriteLine("3. Atrapar pokemon rival");
                //Aqui se valida si tiene mas de un pokemon el usuario, para que pueda hacer un cambio
                if (userPokemonIndex > 1)
                {
                    Console.WriteLine("4. Cambiar pokemon");
                }

            Console.Write("\nOpcion: ");
            int attack = Convert.ToInt32(Console.ReadLine());
            if (attack < 3)
            {
                UserTurn(currentUserPokemon.Name, UserAttack(attack), UserDamage(attack), currentRivalPokemon);
            }
            else if(attack == 3)
            {
                CatchPokemon();
            } 
            else
            {
                UserChangePokemon();
            }
            Console.WriteLine("\nPresiona enter para continuar...");
            string option = Console.ReadLine();
        }
        static int UserDamage(int attack)
        {
            if (attack == 1)
            {
                return currentUserPokemon.AttackDamage1;
            }
            else
            {
                return currentUserPokemon.AttackDamage2;
            }
        }
        //Funcion para saber que ataque va a hacer el usuario
        static string UserAttack(int attack)
        {

            if (attack == 1)
            {
                
                return currentUserPokemon.Attack1;               
            }
            else 
            {
                return currentUserPokemon.Attack2;
            }
        }
        //Esta funcion devuleve 0 mientras el pokemon del usuario tenga HP > 0 y no se haya atrapado todos los pokemones

        static void CatchPokemon()
        {
            Random random = new Random();
            int isCatched = random.Next(0,2); // Devolver 1 o 0
            if (isCatched == 1)
            {
                UserTeam[userPokemonIndex] = currentRivalPokemon;
                IndexRival[0] = -1;
                Console.WriteLine("Atrapaste a: {0}", currentRivalPokemon.Name);
                userPokemonIndex++;
                ChangeRivalPokemon();
            }
            else 
            {
                Console.WriteLine("Fallaste, estuvo cerca!");
            }     
        }
        static void UserChangePokemon()
        {
            int newCurrentUserPokemon;
            Console.WriteLine("Elige tu pokemon para hacer el cambio");
            for (int i = 0; i < userPokemonIndex; i++)
                {
                Console.WriteLine("{0}. {1}",i + 1 ,UserTeam[i].Name);
                }
            Console.Write("Introduce tu eleccion: ");
            newCurrentUserPokemon = Convert.ToInt32(Console.ReadLine());
            currentUserPokemon = UserTeam[newCurrentUserPokemon - 1];
        }

        static int GameFinish()
        {
            if (currentUserPokemon.HP == 0 || currentRivalPokemon.HP == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static void UserTurn(string pokemonUserName,string attackName, int attackDamage, Pokemon currentRivalPokemon)
        {
            Console.WriteLine();
            Console.WriteLine("{0} ha usado {1}!", pokemonUserName, attackName);
            currentRivalPokemon.HP = currentRivalPokemon.HP - attackDamage;
            
        }
        //Sirve para hacer el cambio de pokemon rival, esto sucede si el pokemon actual del rival es atrapado por el usuario
        static void ChangeRivalPokemon()
        {
                //Se cambia el pokemon rival que esta peleando actualmente, por otro pokemon del equipo rival
                currentRivalPokemon = RivalTeam[CallPokemonRival()]; 
        }
        //Funcion para llamar un pokemon rival al azar para pelear
        static int CallPokemonRival()
        {
            int IndexPokemon = random.Next(0, 6);
            int contador = 0;
            while (contador != 6)
            {
                // verifica que el pokemon este disponible.
                if (IndexRival[IndexPokemon] != -1)
                {
                    IndexRival[IndexPokemon] = -1;
                    break;
                }
                else
                {
                    // si la carta no esta disponible, entonces genera otra posicion.
                    IndexPokemon = random.Next(0, 6);
                }
                contador++;
            }
            return IndexPokemon;
        }
        //Este menu solo sale si alguno de los dos pokemones(usuario o rival) tienen vida igual a 0, o el rival se ha quedad sin pokemones
        static void PlayAgain()
        {
            //Reinicia los indices de los pokemones disponibles
            Index = new int[11]; //Arreglo para guardar los indices de pokemons ya utilizados
            IndexRival = new int[6];
            userPokemonIndex = 0;
            int option;
            if (currentRivalPokemon.HP == 0)
            {
                Console.WriteLine("\nPokemon rival: {0}", currentRivalPokemon.Name); ;
                Console.WriteLine("+------------+");
                Console.WriteLine("|    HP: {0}", currentRivalPokemon.HP);
                Console.WriteLine("+------------+");
                Console.WriteLine("\nPokemon ganador: {0}", currentUserPokemon.Name); ;
                Console.WriteLine("+------------+");
                Console.WriteLine("|    HP: {0}", currentUserPokemon.HP);
                Console.WriteLine("+------------+");
                Console.WriteLine("Ganaste!");
                Console.WriteLine("Quieres volver a jugar? Si(1)/No(0)");
                option = Convert.ToInt32(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("\nTu pokemon actual: {0}", currentUserPokemon.Name); ;
                Console.WriteLine("+------------+");
                Console.WriteLine("|    HP: {0}", currentUserPokemon.HP);
                Console.WriteLine("+------------+");
                Console.WriteLine("\nPokemon ganador: {0}", currentRivalPokemon.Name); ;
                Console.WriteLine("+------------+");
                Console.WriteLine("|    HP: {0}", currentRivalPokemon.HP);
                Console.WriteLine("+------------+");
                Console.WriteLine("\nPerdiste!");
                Console.WriteLine("Quieres volver a jugar? Si(1)/No(0)");
                option = Convert.ToInt32(Console.ReadLine());
            }

            if (option == 1)
            {
                Console.Clear();
                StartGame();
            }
            else
            {
                Environment.Exit(1);
            }   
        }
    }
}
