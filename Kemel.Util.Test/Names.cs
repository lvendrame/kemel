using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Util.Test
{
    public class Names
    {
        public static string[] MaleFirstNameList = new string[] {
            "Alexandre", "André", "Anthony", "Antonio", "Arthur", "Augusto", "Benício", "Benjamin", "Bernardo", "Breno", "Bruno", "Bryan",
            "Caio", "Calebe", "Carlos", "Cauã", "Cauê", "César", "Daniel", "Danilo", "Davi", "Diego", "Diogo", "Eduardo", "Emanuel",
            "Enrico", "Enzo", "Erick", "Felipe", "Fernando", "Francisco", "Gabriel", "Giovanni", "Guilherme", "Gustavo", "Heitor",
            "Henrique", "Henry", "Hugo", "Iago", "Ian", "Igor", "Isaac", "João", "Joaquim", "Juan", "Julio", "José", "Kaique","Kevin",
            "Leonardo", "Levi", "Lorenzo", "Luan", "Lucas", "Lucca", "Luiz", "Luís", "Marcelo", "Marcos", "Matheus", "Miguel", "Murilo",
            "Leandro", "Nathan", "Nicolas", "Otávio", "Pedro", "Paulo", "Pietro", "Rafael", "Raul", "Renan", "Renato", "Ricardo",
            "Rodrigo", "Ryan", "Samuel", "Thales", "Theo", "Thiago", "Tomás", "Vinicius", "Vitor", "Yuri"
        };

        public static string[] FemaleFirstNameList = new string[] {
            "Agatha", "Alana", "Alice", "Alícia", "Amanda", "Ana", "Antonella", "Bárbara", "Beatriz", "Betina", "Bianca", "Brenda",
            "Bruna", "Camila", "Carolina", "Caroline", "Catarina", "Cecília", "Clara", "Débora", "Eduarda", "Elisa", "Eloá", "Emanuelly",
            "Emily", "Ester", "Evelyn", "Fernanda", "Gabriela", "Gabrielly", "Giovanna", "Hadassa", "Helena", "Heloisa", "Isabel",
            "Isabella", "Isabelle", "Isadora", "Jennifer", "Joana", "Julia", "Juliana", "Kamilly", "Laís", "Lara", "Larissa", "Laura",
            "Lavínia", "Letícia", "Lívia", "Lorena", "Luana", "Luiza", "Luna", "Maitê", "Manuela", "Marcela", "Maria", "Mariah", "Mariana",
            "Mariane", "Marina", "Melissa", "Michelle", "Milena", "Mirella", "Natália", "Nicole", "Nina", "Olivia", "Pietra", "Rafaela",
            "Raquel", "Rayssa", "Rebeca", "Sabrina", "Sarah", "Sophia", "Sophie", "Stefany", "Stella", "Valentina", "Vitória", "Yasmin"
        };

        public static string[] LastNameList = new string[] {
            "Adães", "Alencar", "Almeida", "Alvarenga", "Álvares", "Alves", "Amaral", "Amorim", "Anes", "Antunes", "Aragão", "Assis",
            "Bandeira", "Barbosa", "Bastos", "Batista", "Bermudes", "Bernardes", "Borges", "Botelho", "Braga", "Bragança", "Brandão",
            "Bueno", "Cabral", "Caminha", "Camões", "Cardoso", "Castro", "Cerejeira", "Chaves", "Coelho", "Coentrão", "Costa", "Couto",
            "Coutinho", "Cruz", "Cunha", "Dambros", "Dias", "Diegues", "Duarte", "Eça", "Esteves", "Faleiros", "Farias", "Faro", "Ferrão",
            "Ferreira", "Ferrolho", "Fernandes", "Figo", "Figueira", "Figueiredo", "Figueiroa", "Fonseca", "Fontes", "Fortaleza", "Furquim",
            "Galvão", "Gomes", "Gonçales", "Gonçalves", "Gouveia", "Guimarães", "Gusmão", "Henriques", "Lacerda", "Leme", "Lins", "Macedo",
            "Machado", "Magalhães", "Malafaia", "Mariz", "Marques", "Martins", "Massa", "Matos", "Meireles", "Mendes", "Mendonça", "Menendes",
            "Miranda", "Moraes", "Morais", "Moreira", "Monteiro", "Muniz", "Naves", "Negreiros", "Neves", "Nogueira", "Noronha", "Nunes",
            "Oliva", "Oliveira", "Paes", "Pais", "Pascoal", "Pasquim", "Pedrosa", "Pedroso", "Peixoto", "Pereira", "Peres", "Pimenta", "Pinto",
            "Pires", "Policarpo", "Queirós", "Queiroz", "Ramalho", "Ramires", "Ramos", "Reis", "Ribeiro", "Rios", "Rodrigues", "Sá", "Sampaio",
            "Santana", "Santiago", "Santos", "Saraiva", "Saramago", "Silva", "Silveira", "Simões", "Siqueira", "Soares", "Soeiro", "Sousa",
            "Souza", "Tavares", "Teixeira", "Vasconcelos", "Vasques", "Vaz", "Veiga", "Velasques", "Veloso", "Viana", "Vieira"
        };

        public static string[] CompanyTypes = new string[] {
            "Ltda.", "ME", "S/A"
        };

        public enum Gender
        {
            Male,
            Female
        }

        public static string GetFirstName(int qtd, Gender gender)
        {
            StringBuilder stb = new StringBuilder();
            string[] list = GetGenderList(gender);

            for (int i = 0; i < qtd; i++)
			{
                if(i > 0)
                    stb.Append(" ");

                stb.Append(list[Randonizer.NextInt(list.Length - 1)]);
			}

            return stb.ToString();
        }

        public static string GetFirstName(Gender gender)
        {
            return GetFirstName(Randonizer.NextInt(1, 3), gender);
        }

        public static string GetFirstName(int qtd)
        {
            Gender gender = Gender.Male;
            if (Randonizer.NextInt(1, 3) == 2)
                gender = Gender.Female;

            return GetFirstName(qtd, gender);
        }

        public static string GetFirstName()
        {
            Gender gender = Gender.Male;
            if (Randonizer.NextInt(1, 3) == 2)
                gender = Gender.Female;

            return GetFirstName(Randonizer.NextInt(1, 3), gender);
        }

        public static string GetLastName(int qtd)
        {
            StringBuilder stb = new StringBuilder();

            for (int i = 0; i < qtd; i++)
            {
                if (i > 0)
                    stb.Append(" ");

                stb.Append(LastNameList[Randonizer.NextInt(LastNameList.Length - 1)]);
            }

            return stb.ToString();
        }

        public static string GetLastName()
        {
            return GetLastName(Randonizer.NextInt(1, 3));
        }

        public static string GetFullName(Gender gender)
        {
            return string.Concat(GetFirstName(gender), " ", GetLastName());
        }

        public static string GetFullName()
        {
            return string.Concat(GetFirstName(), " ", GetLastName());
        }

        private static string[] GetGenderList(Gender gender)
        {
            if (gender == Gender.Male)
                return MaleFirstNameList;
            return FemaleFirstNameList;
        }

        public static string GetCompanyName()
        {
            return string.Concat(GetLastName(Randonizer.NextInt(2, 5)), " ", CompanyTypes[Randonizer.NextInt(CompanyTypes.Length)]);
        }

    }
}
