using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedCollects(CollectAppContext context, int quantidade = 200)
    {
        if (await context.Collects.AnyAsync())
        {
            Console.WriteLine("⚠️  Banco já possui dados de coletas. Nenhuma coleta foi adicionada.");
            return;
        }

        var random = new Random();
        var collects = new List<Collect>();

        var userIds = await context.Users.Select(u => u.Id).ToListAsync();
        var supplierIds = await context.Suppliers.Select(s => s.Id).ToListAsync();
        var productIds = await context.Products.Select(p => p.Id).ToListAsync();
        var filialIds = await context.Filials.Select(f => f.Id).ToListAsync();

        if (!userIds.Any())
        {
            Console.WriteLine("❌ ERRO: Nenhum usuário encontrado! Cadastre usuários primeiro.");
            return;
        }
        if (!supplierIds.Any())
        {
            Console.WriteLine("❌ ERRO: Nenhum fornecedor encontrado! Cadastre fornecedores primeiro.");
            return;
        }
        if (!productIds.Any())
        {
            Console.WriteLine("❌ ERRO: Nenhum produto encontrado! Cadastre produtos primeiro.");
            return;
        }
        if (!filialIds.Any())
        {
            Console.WriteLine("❌ ERRO: Nenhuma filial encontrada! Cadastre filiais primeiro.");
            return;
        }

        Console.WriteLine($"🔄 Gerando {quantidade} coletas fictícias...");

        var dataInicio = DateTime.Now.AddMonths(-6);
        var dataFim = DateTime.Now;

        for (int i = 0; i < quantidade; i++)
        {
            var diasAleatorios = random.Next(0, 180);
            var collectAt = dataInicio.AddDays(diasAleatorios);
            
            var createdAt = collectAt.AddDays(-random.Next(1, 10));

            CollectStatus status;
            var statusRandom = random.Next(100);
            if (statusRandom < 15)
                status = CollectStatus.PendenteAprovar;
            else if (statusRandom < 30)
                status = CollectStatus.PendenteColetar;
            else if (statusRandom < 85)
                status = CollectStatus.Coletado;
            else
                status = CollectStatus.Cancelado;

            var collect = new Collect
            {
                CreatedAt = createdAt,
                CollectAt = collectAt,
                Volume = random.Next(50, 500),
                Weight = random.Next(30, 300),
                Status = status,
                UserId = userIds[random.Next(userIds.Count)],
                SupplierId = supplierIds[random.Next(supplierIds.Count)],
                ProductId = productIds[random.Next(productIds.Count)],
                FilialId = filialIds[random.Next(filialIds.Count)]
            };

            collects.Add(collect);
        }

        await context.Collects.AddRangeAsync(collects);
        await context.SaveChangesAsync();

        Console.WriteLine($"✅ {quantidade} coletas inseridas com sucesso!");
        Console.WriteLine($"📊 Distribuição:");
        Console.WriteLine($"   - Pendente Aprovar: {collects.Count(c => c.Status == CollectStatus.PendenteAprovar)}");
        Console.WriteLine($"   - Pendente Coletar: {collects.Count(c => c.Status == CollectStatus.PendenteColetar)}");
        Console.WriteLine($"   - Coletado: {collects.Count(c => c.Status == CollectStatus.Coletado)}");
        Console.WriteLine($"   - Cancelado: {collects.Count(c => c.Status == CollectStatus.Cancelado)}");
    }

    public static async Task ClearCollects(CollectAppContext context)
    {
        var collects = await context.Collects.ToListAsync();
        context.Collects.RemoveRange(collects);
        await context.SaveChangesAsync();
        Console.WriteLine($"🗑️  {collects.Count} coletas removidas do banco.");
    }
}