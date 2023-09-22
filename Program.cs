using Microsoft.EntityFrameworkCore;
using Jokes.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddDbContext<JokesContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("SQLiteCS")))
 // .AddDbContext<JokesContext>(opt => opt.UseInMemoryDatabase("Catalog"))
    ;

// Learn more about configring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => 
{
    o.AddDefaultPolicy(p => 
    {
        p
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    BootstrapData(scope);
}
app.UseCors();
app.Run();

static void BootstrapData(IServiceScope scope)
{
    var service = scope.ServiceProvider;
    var context = service.GetService<JokesContext>();

    if (context is null)
    {
        return;
    }

    if(context.Categories.Count()==0){
        context.Categories.Add(new Category { Name="Alentejanos" });
        context.Categories.Add(new Category { Name="Futebol" });
        context.Categories.Add(new Category { Name="Joaozinho" });
        context.Categories.Add(new Category { Name="Loiras" });
        context.Categories.Add(new Category { Name="Sogras" });
        context.SaveChanges();
    }

    if(context.Jokes.Count()==0){
        context.Jokes.Add(new Joke { Category=context.Categories.Find(1L), Text="Quantos alentejanos são precisos para conduzir uma ambulância? 3! Um para a conduzir Um para gritar NIIII NOOOO NIII NOOO E outro que roda a cabeça e grita: AZZULLLIIII... VERMÊÊÊLHOOOOO... AZZULLLIIII... VERMÊÊÊLHOOOO...." });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(1L), Text="Como é que se faz rir um Alentejano na Segunda-Feira? Conta-lhe uma piada na Sexta..." });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(2L), Text="Veloso (Benfica) Isto passou-se em 95, quando os jogadores do Benfica entraram num suposto Blackout informativo. Veloso, como capitão, teve a honra de ler o comunicado e, pelos vistos, nem isso soube fazer: Os jogadores do Benfica permanecerão em blackout enquanto toda a verdade não for RESPOSTA." });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(2L), Text="Repórter: - João Pinto, felicidades para o jogo. J- Obrigado, igualmente." });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(3L), Text="O Joãozinho chega em casa e diz ao seu pai:\n- Pai, hoje recebi as notas!\n- Emprestei-as!\n- Mas porquê?\n- Então, onde estão elas? — disse o pai.\n- Porque o meu amigo queria assustar o pai dele." });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(3L), Text="A professora pergunta ao Joãozinho:\n- Joãozinho, por que é que não fizeste os trabalhos de casa?\nE ele prontamente respondeu:\n- Ora, porque eu moro num apartamento." });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(4L), Text="Num autocarro de 2 andares iam as morenas em baixo e as loiras em cima. As morenas iam na maior festa enquanto das loiras não se ouvia um pio. Então diz uma morena para a outra:\n - Olha lá! Vai lá ver o que é que se passa com as loiras!\nEla subiu ao primeiro andar e viu as loiras todas agarradas umas às outras a tremer, então ela pergunta a uma loira:\n - Olha lá, o que é que se passa, porque é que estão tão caladas?\n - Pois! Vocês têm condutor, nós não!" });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(4L), Text="Como as loiras tentam matar um peixe? Afogando-o!" });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(5L), Text="O homem estava assistindo ao jornal nacional quando, de repente, uma notícia o interessou. Falava de um homem que matou a sogra e a enterrou no chão da sala e só agora, 25 anos depois, é que descobriram. O gajo ficou a pensar muito naquilo.\n - Eu também poderia matar a megera da minha sogra e enterrá-la na sala. Até descobrirem, já estarei morto, pois tenho 50 anos...E, acho que vou fazer isso sim, raios!\n E armou a armadilha. Chamou a sogra para um jantar. Na primeira oportunidade, BAM! Lenhada na cabeça da velha, que logo foi enterrada na sala. Meia hora depois, toca a campainha do homem. Era a polícia, que avisou:\n - O Sr está preso por assassinar a sua sogra!\n - Mas, mas, mas...\n - Nada de mas, já para o carro!\nNa esquadra, o gajo, desconsolado, esbracejava:\n - Eu vi na TV, um homem fez a mesma coisa e demorou 25 anos a ser descoberto! Como é que vocês me descobriram tão rapidamente???\n - O truque é que ele não morava no segundo andar..." });
        context.Jokes.Add(new Joke { Category=context.Categories.Find(5L), Text="À porta do cemitério há uma grande bicha de homens. Por curiosidade ele vai perguntar o que é. Responde-lhe um homem com um lobo d'alsácia à trela: - Foi o enterro da minha sogra, mordida na garganta por este meu cão. - Ah! - Diz o outro, um pouco espantado - Olhe lá! E o senhor não me quer vender esse belo animal? - Bem. Talvez se chegue a um acordo. Entre aí na bicha e espere pela sua vez." });
        context.SaveChanges();
    }
    
}

