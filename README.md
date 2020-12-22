# Hubee Caching Sdk

![N|Solid](https://media-exp1.licdn.com/dms/image/C4E0BAQHOp41isf2byw/company-logo_200_200/0?e=1611792000&v=beta&t=R627Tkw1cwQgb-LjNTJh_4auJWQsQieuU4wHoyLfIDA)

Hubee Caching Sdk é uma biblioteca que faz abstração da implementação de cache distribuído para as aplicações. A principal ideia desse SDK é abstrair toda a complexidade das configurações e ser adaptável para as mudanças de tecnologias, centralizando toda manutenção e evolução em um único ponto.

## Service cache implementados

- Redis

## Getting started

Após realizar a instalação do SDK em seu projeto podemos iniciar a configuração para utilizá-lo, segue abaixo a configuração que deve ser realizada na seção **"HubeeCachingConfig"** dentro do arquivo appsettings:

```json
"HubeeCachingConfig": {
    "CacheProvider": "Redis",
    "DefaultExpiresIn": "00:01:01",
    "Host": "localhost",
    "Port": "3000",
    "Password": "password"
}
```

| Configuração | Observação |
|:----|:----------|
| CacheProvider | tipo do provedor |
| DefaultExpiresIn | tempo padrão para a expiração do cache, valor em TimeSpan |
| Host | host do provedor de cache |
| Port | porta do provedor de cache |
| Password | senha do provedor de cache |

**OBS:** as configurações de acesso ao provedor podem ser configuradas em variáveis de ambiente, segue abaixo os nomes:

```yml
HUBEE_CACHING_HOST=valor
HUBEE_CACHING_PORT=valor
HUBEE_CACHING_PASSWORD=valor
```

Depois da configuração acima deve-se adicionar o SDK na aplicação,
segue abaixo a linha de código que deve ser adicionada no arquivo "Startup.cs":

```csharp
using Hubee.Caching.Sdk.Core.Extensions;

public class Startup
{
  //(...)
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddCaching(Configuration);
  }
}
```

## Cache

Segue abaixo os passos para realizar a utilização do cache:

```csharp
using Hubee.Caching.Sdk.Core.Interfaces;
//(...)

private readonly ICachingService _cachingService;

public Sample(ICachingService eventBusService)
{
  _cachingService = cachingService;
}

//(...)

var key = "123456@";
var value = new Entidade
{
  Id = Guid.NewGuid(),
  Date = DateTime.Now
};

//adicionar informação no cache
await _cachingService.Set<Entidade>(key, value);

//obter informação do cache
var valorDoCache = await _cachingService.Get<Entidade>(key);

```

Nó método **SET** podemos adicionar a informação de expiração na propriedade **expiresIn**, caso não desejar informar a expiração será determinada pelo seu valor padrão definido na configuração.
