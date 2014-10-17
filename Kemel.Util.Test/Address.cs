using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Util.Test
{
    public class Address
    {
        public static string[][] Cities = new string[][]{
                new string[]{"Acrelandia", "Assis Brasil", "Brasileia", "Bujari", "Manoel Urbano", "Marechal Thaumaturgo", "Placido de Castro", "Porto Acre", "Porto Walter", "Rio Branco"},
                new string[]{"Agua Branca", "Alazao", "Alecrim", "Maceió", "Palmeira de Fora", "Palmeira dos Indios", "Pau Ferro", "Paulo Jacinto", "Penedo", "Piacabucu"},
                new string[]{"Alvaraes", "Amatari", "Amatura", "Manaus", "Manicore", "Silves", "Tabatinga", "Tapaua", "Tefe", "Vila Pitinga"},
                new string[]{"Abacate da Pedreira", "Agua Branca do Amapari", "Amapa", "Macapa", "Mazagao", "Mazagao Velho", "Oiapoque", "Paredao", "Porto Grande", "Vitoria do Jari"},
                new string[]{"Abadia", "Salgadalia", "Salinas da Margarida", "Salobrinho", "Salobro", "Salvador", "Vale Verde", "Wanderley", "Wenceslau Guimaraes", "Xique-xique"},
                new string[]{"Abaiara", "Abilio Martins", "Acarape", "Acarau", "Acopiara", "Adrianopolis", "Agua Verde", "Fortaleza", "Vicosa do Ceara", "Vila Soares"},
                new string[]{"Brasilia", "Brazlandia", "Candangolandia", "Ceilandia", "Cruzeiro", "Gama", "Guara", "Lago Norte", "Lago Sul", "Nucleo Bandeirante"},
                new string[]{"Acioli", "Afonso Claudio", "Alto Santa Maria", "Guarapari", "Guararema", "Ibatiba", "Ibes", "Vinhatico", "Vinte E Cinco de Julho", "Vitoria"},
                new string[]{"Caldas Novas", "Firminopolis", "Flores de Goias", "Goiania", "Goianira", "Goias", "Vila Boa", "Vila Brasilia", "Vila Propicio", "Vila Sertaneja"},
                new string[]{"Acailandia", "Afonso Cunha", "Nova Iorque", "Nova Olinda do Maranhao", "Olho Dagua Das Cunhas", "Sao Luis", "Sao Luis Gonzaga do Maranhao", "Vitoria do Mearim", "Vitorino Freire", "Ze Doca"},
                new string[]{"Abadia dos Dourados", "Abaete", "Abaete dos Mendes", "Abaiba", "Belo Horizonte", "Cachoeira de Pajeu", "Volta Grande", "Wenceslau Braz", "Zelandia", "Zito Soares"},
                new string[]{"Agua Boa", "Agua Clara", "Albuquerque", "Alcinopolis", "Campo Grande", "Selviria", "Sete Quedas", "Vila Uniao", "Vila Vargas", "Vista Alegre"},
                new string[]{"Acorizal", "Agua Boa", "Agua Fria", "Aguacu", "Cuiaba", "Vila Operaria", "Vila Paulista", "Vila Progresso", "Vila Rica", "Vila Santo Antonio"},
                new string[]{"Abaetetuba", "Abel Figueiredo", "Belem", "Curralinho", "Lauro Sodre", "Ligacao do Para", "Maraba", "Paratins", "Vitoria do Xingu", "Xinguarinha"},
                new string[]{"Agua Branca", "Aguiar", "Alagoa Grande", "Jerico", "Joao Pessoa", "Juarez Tavora", "Riacho de Santo Antonio", "Riacho dos Cavalos", "Vista Serrana", "Zabele"},
                new string[]{"Abreu E Lima", "Afogados da Ingazeira", "Afranio", "Agrestina", "Recife", "Vitoria de Santo Antao", "Volta do Moxoto", "Xexeu", "Xucuru", "Ze Gomes"},
                new string[]{"Acaua", "Agricolandia", "Agua Branca", "Beneditinos", "Bertolinia", "Teresina", "Varzea Grande", "Vera Mendes", "Vila Nova do Piaui", "Wall Ferraz"},
                new string[]{"Abapa", "Abatia", "Curitiba", "Foz do Iguacu", "Ilha do Mel", "Londrina", "Maringa", "Mariopolis", "Xambre", "Yolanda"},
                new string[]{"Armacao de Buzios", "Arraial do Cabo", "Belford Roxo", "Cabo Frio", "Duque de Caxias", "Niteroi", "Parati", "Petropolis", "Rio de Janeiro", "Xerem"},
                new string[]{"Acu", "Afonso Bezerra", "Ceara-mirim", "Cerro Cora", "Coronel Ezequiel", "Mossoro", "Natal", "Vera Cruz", "Vicosa", "Vila Flor"},
                new string[]{"Abuna", "Alta Alegre dos Parecis", "Alta Floresta do Oeste", "Jaru", "Ji-parana", "Porto Velho", "Sao Felipe Doeste", "Vila Extrema", "Vilhena", "Vista Alegre do Abuna"},
                new string[]{"Alto Alegre", "Amajari", "Boa Vista", "Bonfim", "Canta", "Caracarai", "Normandia", "Pacaraima", "Rorainopolis", "Uiramuta"},
                new string[]{"Acegua", "Afonso Rodrigues", "Agua Santa", "Cachoeira do Sul", "Gramado", "Porto Alegre", "Volta Grande", "Xadrez", "Xangri-la", "Xingu"},
                new string[]{"Abdon Batista", "Abelardo Luz", "Agrolandia", "Blumenau", "Bocaina do Sul", "Florianopolis", "Xanxere", "Xavantina", "Xaxim", "Zortea"},
                new string[]{"Altos Verdes", "Aracaju", "Araua", "Areia Branca", "Frei Paulo", "Gararu", "General Maynard", "Tobias Barreto", "Tomar do Geru", "Umbauba"},
                new string[]{"Diadema", "Itapecerica da Serra", "Itapetininga", "Maua", "Novo Horizonte", "Santo Andre", "Sao Bernardo do Campo", "Sao Caetano do Sul", "Sao Paulo", "Sao Vicente"},
                new string[]{"Abreulandia", "Aguiarnopolis", "Alianca do Tocantins", "Novo Jardim", "Oliveira de Fatima", "Palmas", "Tupiratins", "Venus", "Wanderlandia", "Xambioa"}
        };

        public static string[] DistrictPrefix = new string[] {
            "Bairro", "Jardim", "Vila"
        };

        public static string[] StreetPrefix = new string[] {
            "Alameda", "Área", "Avenida", "Campo", "Chácara", "Colônia", "Condomínio", "Conjunto", "Distrito", "Esplanada", "Estação",
            "Estrada", "Favela", "Feira", "Jardim", "Ladeira", "Lago", "Lagoa", "Largo", "Loteamento", "Morro", "Núcleo", "Parque",
            "Passarela", "Pátio", "Praça", "Quadra", "Recanto", "Residencial", "Rodovia", "Rua", "Setor", "Sítio", "Travessa", "Trecho",
            "Trevo", "Vale", "Vereda", "Via", "Viaduto", "Viela", "Vila"
        };

        public static States GetRandomState()
        {
            return (States)Randonizer.NextInt(28);
        }

        public static string GetStateName(States state)
        {
            switch (state)
            {
                case States.AC: return "Acre";
                case States.AL: return "Alagoas";
                case States.AP: return "Amapá";
                case States.AM: return "Amazonas";
                case States.BA: return "Bahia";
                case States.CE: return "Ceará";
                case States.DF: return "Distrito Federal";
                case States.ES: return "Espírito Santo";
                case States.GO: return "Goiás";
                case States.MA: return "Maranhão";
                case States.MT: return "Mato Grosso";
                case States.MS: return "Mato Grosso do Sul";
                case States.MG: return "Minas Gerais";
                case States.PA: return "Pará";
                case States.PB: return "Paraíba";
                case States.PR: return "Paraná";
                case States.PE: return "Pernambuco";
                case States.PI: return "Piauí";
                case States.RJ: return "Rio de Janeiro";
                case States.RN: return "Rio Grande do Norte";
                case States.RS: return "Rio Grande do Sul";
                case States.RO: return "Rondônia";
                case States.RR: return "Rorâima";
                case States.SC: return "Santa Catarina";
                case States.SP: return "São Paulo";
                case States.SE: return "Sergipe";
                case States.TO: return "Tocantins";
            }
            return string.Empty;
        }

        public static string GetAnyCityFromState(States state)
        {
            int ct = Randonizer.NextInt(10);
            return Cities[(int)state][ct];
        }

        public static string GenerateZipNumber()
        {
            StringBuilder stb = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                stb.Append(Randonizer.NextInt(10));
            }

            return stb.Insert(5, "-").ToString();
        }

        public static string GenerateStreetName()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append(StreetPrefix[Randonizer.NextInt(StreetPrefix.Length)]);
            stb.Append(" ");
            stb.Append(Names.GetFullName());
            return stb.ToString();
        }

        public static string GenerateDistrictName()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append(DistrictPrefix[Randonizer.NextInt(DistrictPrefix.Length)]);
            stb.Append(" ");
            stb.Append(Names.GetLastName(Randonizer.NextInt(1,3)));
            return stb.ToString();
        }
    }
}





























