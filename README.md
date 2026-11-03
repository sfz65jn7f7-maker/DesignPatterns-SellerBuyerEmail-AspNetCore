\# Exercice pratique : les design patterns en C# / ASP.NET Core



Cet exercice sert à \*\*pratiquer\*\* C#, ASP.NET Core, EF Core, l'architecture MVC, les API REST, les principes SOLID et les bonnes pratiques (injection de dépendances, couplage faible), avec un objectif principal : \*\*appliquer et comprendre le maximum de design patterns\*\* (comment, pourquoi et quand les utiliser).



\## Énoncé de départ



Un site web gère l'inscription de \*\*vendeurs\*\* et d'\*\*acheteurs\*\*, et doit envoyer un \*\*email de confirmation\*\*. L'objet Email n'a pas d'état et coûte cher à créer. Trois classes sont à créer au départ :



\- `Vendeur` (Développeur 1)

\- `Acheteur` (Développeur 2)

\- `EmailSender` (Développeur 3)



L'énoncé impose le \*\*Singleton\*\* pour `EmailSender`. Les autres patterns ont été ajoutés pour s'exercer.



\## Stack utilisée



\- C# et .NET 10 (`net10.0`)

\- ASP.NET Core (API REST avec controllers)

\- Entity Framework Core 10 (Fluent API, migrations)

\- SQL Server (LocalDB)

\- Postman pour les tests

\- Visual Studio



\## Structure



```

InscriptionApp.Api/

├── Adapters/          ExternalMailClient, ExternalMailAdapter

├── Builders/          EmailMessageBuilder

├── Commands/          ICommand, ICommandHandler, InscrireVendeurCommand, InscrireAcheteurCommand

│   └── Handlers/      InscrireVendeurCommandHandler, InscrireAcheteurCommandHandler

├── Controllers/       VendeursController, AcheteursController

├── Data/              AppDbContext

│   └── Configurations/  VendeurConfiguration, AcheteurConfiguration

├── Decorators/        LoggingEmailSender, RetryEmailSender

├── DTOs/

│   ├── Request/       UtilisateurCreateRequest, VendeurCreateRequest, AcheteurCreateRequest

│   └── Response/      UtilisateurResponse, VendeurResponse, AcheteurResponse

├── Enums/             StatutInscription, ActionStatut, CanalNotification, Langue

├── Events/            UtilisateurInscritEvent, IEventHandler, IEventPublisher, EventPublisher

│   └── Handlers/      EnvoyerConfirmationHandler, JournaliserInscriptionHandler

├── Exceptions/        NotFoundException, EmailDejaUtiliseException,

│                      EmailSendingException, TransitionEtatInvalideException

├── Factories/         ConfirmationMessageFactory, VendeurMessageFactory, AcheteurMessageFactory

│   └── Kits/          INotificationKitFactory, KitFrancaisFactory, KitAnglaisFactory,

│                      IGabaritMessage, GabaritFrancais, GabaritAnglais,

│                      IFormateurDate, FormateurDate, NotificationKitProvider

├── Filters/           GlobalExceptionFilter

├── Mappers/           IMapper, VendeurMapper, AcheteurMapper

├── Mediator/          IMediator, CommandMediator

├── Migrations/        migrations EF Core

├── Models/            Utilisateur (abstraite), Vendeur, Acheteur, EmailMessage

├── Options/           SmtpOptions

├── Proxies/           RateLimitedEmailSenderProxy

├── Repositories/

│   ├── Interfaces/    IRepository<T>, IVendeurRepository, IAcheteurRepository

│   └── Implementations/  VendeurRepository, AcheteurRepository

├── Services/

│   ├── Interfaces/    IVendeurService, IAcheteurService, IEmailSender, IInscriptionFacade

│   └── Implementations/

│       ├── Base/      InscriptionServiceBase

│       └── VendeurService, AcheteurService, EmailSender, InscriptionFacade

├── States/            IInscriptionState, InscriptionStateBase, EnAttenteState, ConfirmeeState,

│                      SuspendueState, InscriptionStateFactory, InscriptionContext

├── Strategies/        INotificationStrategy, EmailNotificationStrategy,

│                      SmsNotificationStrategy, NotificationContext

├── appsettings.json

├── InscriptionApp.Api.http

└── Program.cs

```



\## Logique et fonctionnement



\### Parcours d'une inscription



```

Postman

&#x20;  │  POST /api/vendeurs

&#x20;  ▼

Controller ──► Facade ──► Mediator ──► Handler de commande ──► Service (Template Method)

&#x20;                                                                   │

&#x20;                         1. valider (email unique)                 │

&#x20;                         2. convertir DTO → entité (Mapper)        │

&#x20;                         3. enregistrer (Repository → SQL Server)  │

&#x20;                         4. publier « UtilisateurInscrit » ────────┘

&#x20;                         5. renvoyer la réponse (201)



Événement « UtilisateurInscrit » (Observer) ──► deux abonnés :

&#x20;  ├─ EnvoyerConfirmationHandler

&#x20;  │     kit de langue (Abstract Factory) ► message (Factory Method + Builder)

&#x20;  │     ► canal d'envoi (Strategy) ► chaîne IEmailSender

&#x20;  └─ JournaliserInscriptionHandler ► écrit « Nouvelle inscription : ... »

```



\### Chaîne d'envoi d'email



```

LoggingEmailSender (Decorator)

&#x20;  └─► RateLimitedEmailSenderProxy (Proxy)

&#x20;         └─► RetryEmailSender (Decorator)

&#x20;                └─► EmailSender (Singleton)  ou  ExternalMailAdapter (Adapter)

```



Le Proxy est placé \*\*avant\*\* le Retry : un refus pour limite atteinte n'est ainsi pas réessayé inutilement. Le choix entre `EmailSender` et l'Adapter se fait avec le réglage `Smtp:Provider`.



\### Cycle de vie du statut (State)



```

EnAttente ──confirmer──► Confirmee ──suspendre──► Suspendue

&#x20;   │                                                 │

&#x20;   └──────────────suspendre──────────────────────────┘

&#x20;                                  Suspendue ──réactiver──► Confirmee

```



Toute autre transition est refusée avec une réponse \*\*409\*\*.



\### Durées de vie en injection de dépendances



\- \*\*Singleton\*\* : `EmailSender` et sa chaîne, mappers, factories de messages, kits de langue, stratégies. Ils n'ont pas d'état propre à une requête (le seul compteur, celui du Proxy, est protégé par un `lock`).

\- \*\*Scoped\*\* : `AppDbContext`, repositories, services, publieur d'événements, Mediator, Facade et handlers. Ils dépendent du `DbContext` ou le font transiter.



Un Singleton ne doit jamais dépendre d'un service Scoped.



\## Design patterns utilisés (14)



\### Création



| Pattern | Rôle | Apport dans l'exercice | Dossiers |

|---|---|---|---|

| \*\*Singleton\*\* | Garantit une seule instance d'une classe | `EmailSender` est créé une seule fois et partagé (obligatoire d'après l'énoncé). Un identifiant d'instance dans les logs le prouve | `Services/Implementations`, `Program.cs` |

| \*\*Factory Method\*\* | Délègue la création d'un objet à des classes spécialisées | Chaque type d'utilisateur fabrique son propre message de confirmation | `Factories` |

| \*\*Abstract Factory\*\* | Crée une famille d'objets compatibles | Un kit par langue fournit un gabarit et un formateur de date cohérents entre eux | `Factories/Kits` |

| \*\*Builder\*\* | Construit un objet complexe étape par étape | `EmailMessageBuilder` assemble le message et valide dans `Build()` | `Builders` |



\### Structure



| Pattern | Rôle | Apport dans l'exercice | Dossiers |

|---|---|---|---|

| \*\*Adapter\*\* | Fait collaborer deux interfaces incompatibles | Traduit l'API d'un fournisseur externe simulé vers `IEmailSender` | `Adapters` |

| \*\*Decorator\*\* | Ajoute un comportement sans modifier la classe | Journalisation et nouvelles tentatives autour de l'envoi | `Decorators` |

| \*\*Proxy\*\* | Contrôle l'accès à un objet | Limite le nombre d'emails par minute avant d'atteindre l'expéditeur | `Proxies` |

| \*\*Facade\*\* | Simplifie l'accès à un système complexe | Les controllers appellent `InscrireVendeurAsync` sans connaître commandes et Mediator | `Services/Implementations`, `Services/Interfaces` |



\### Comportement



| Pattern | Rôle | Apport dans l'exercice | Dossiers |

|---|---|---|---|

| \*\*Template Method\*\* | Fixe le squelette d'un algorithme, les sous-classes complètent | `InscriptionServiceBase` fige les 5 étapes d'inscription, Vendeur et Acheteur ne fournissent que la vérification d'email | `Services/Implementations/Base` |

| \*\*Observer\*\* | Notifie plusieurs abonnés lors d'un changement | Le service annonce l'inscription, un abonné envoie l'email, un autre journalise. L'échec de l'un ne bloque pas l'autre | `Events`, `Events/Handlers` |

| \*\*Strategy\*\* | Rend des algorithmes interchangeables | Le canal d'envoi (email ou SMS simulé) est choisi à l'exécution | `Strategies` |

| \*\*State\*\* | Change le comportement selon l'état | Chaque statut sait quelles transitions il accepte, sans `if` sur le statut | `States` |

| \*\*Command\*\* | Encapsule une action dans un objet | L'inscription devient `InscrireVendeurCommand` ou `InscrireAcheteurCommand` | `Commands`, `Commands/Handlers` |

| \*\*Mediator\*\* | Centralise les interactions entre objets | Transmet chaque commande au bon handler | `Mediator` |



\### Déjà présents sans effort supplémentaire



\- \*\*Chain of Responsibility\*\* : les middlewares et filtres d'ASP.NET Core (`Filters/GlobalExceptionFilter`).

\- \*\*Repository\*\* (hors catalogue GoF) : `Repositories`.



\## Principes SOLID et bonnes pratiques



\- \*\*S\*\* : une classe, une responsabilité (mapper, repository, service, handler).

\- \*\*O\*\* : ajouter un canal, une langue ou un décorateur ne modifie pas l'existant.

\- \*\*L\*\* : toute implémentation (EmailSender, Adapter, Decorators, Proxy) respecte `IEmailSender`.

\- \*\*I\*\* : petites interfaces ciblées (`IRepository<T>` pour le commun, interfaces spécifiques pour le reste).

\- \*\*D\*\* : les classes dépendent des interfaces, injectées par constructeur.

\- DTOs séparés en `Request` et `Response`, les entités ne sortent jamais de l'API.

\- Gestion centralisée des erreurs avec `GlobalExceptionFilter` et `ProblemDetails`.

\- Configuration typée avec `IOptions<SmtpOptions>`.



\## Endpoints



```

POST  /api/vendeurs                     inscrit un vendeur

GET   /api/vendeurs                     liste les vendeurs

GET   /api/vendeurs/{id}                détail d'un vendeur

POST  /api/vendeurs/{id}/confirmer      EnAttente → Confirmee

POST  /api/vendeurs/{id}/suspendre      → Suspendue

POST  /api/vendeurs/{id}/reactiver      Suspendue → Confirmee



POST  /api/acheteurs                    inscrit un acheteur

GET   /api/acheteurs

GET   /api/acheteurs/{id}

POST  /api/acheteurs/{id}/confirmer

POST  /api/acheteurs/{id}/suspendre

POST  /api/acheteurs/{id}/reactiver

```



Corps d'inscription d'un vendeur (`langue` est optionnelle, `Fr` par défaut, valeurs `Fr` ou `En`) :



```json

{

&#x20; "nom": "Benali",

&#x20; "prenom": "Sara",

&#x20; "email": "sara@mail.com",

&#x20; "nomBoutique": "Chez Sara",

&#x20; "langue": "Fr"

}

```



Corps d'inscription d'un acheteur :



```json

{

&#x20; "nom": "Alaoui",

&#x20; "prenom": "Youssef",

&#x20; "email": "youssef@mail.com",

&#x20; "adresseLivraison": "12 rue Hassan II, Oujda",

&#x20; "langue": "En"

}

```



\## Configuration



Section `Smtp` d'`appsettings.json` :



```json

"Smtp": {

&#x20; "Enabled": false,

&#x20; "Provider": "Smtp",

&#x20; "MaxRetryAttempts": 3,

&#x20; "MaxEmailsPerMinute": 30,

&#x20; "Host": "smtp.example.com",

&#x20; "Port": 587,

&#x20; "EnableSsl": true,

&#x20; "Username": "",

&#x20; "Password": "",

&#x20; "SenderAddress": "no-reply@inscriptionapp.local"

}

```



\- `Enabled: false` : aucun email réel n'est envoyé, le contenu est simplement écrit dans la console.

\- `Provider` : `Smtp` (EmailSender) ou `External` (Adapter).

\- `MaxRetryAttempts` : nombre de tentatives du Retry.

\- `MaxEmailsPerMinute` : seuil du Proxy.



Ne jamais versionner de vrai mot de passe SMTP.



\## Exécuter l'exercice



\### Prérequis



\- SDK .NET 10

\- SQL Server LocalDB (installé avec Visual Studio) ou une autre instance SQL Server

\- Outil `dotnet-ef` : `dotnet tool install --global dotnet-ef`

\- Postman, ou le fichier `InscriptionApp.Api.http` avec Visual Studio



\### Commandes



```powershell

dotnet restore

dotnet ef database update

dotnet run

```



L'API écoute sur `http://localhost:5255`. Avec une autre instance SQL Server, adapter `ConnectionStrings:DefaultConnection` dans `appsettings.json`.



\## Déroulement de l'exercice et tests essentiels



L'exercice a été construit en sept étapes. Après chaque étape, des requêtes Postman et la console de `dotnet run` ont servi à vérifier le comportement.



\### Étape 1 : modèles, EF Core, repositories



Création de `Utilisateur`, `Vendeur`, `Acheteur`, `AppDbContext`, des configurations Fluent API (index unique sur l'email) et des repositories, puis migration `InitialCreate`.



\### Étape 2 : Singleton et Builder



Ajout de `IEmailSender`, `EmailSender` et `EmailMessageBuilder`.



\### Étape 3 : Template Method, Factory Method, services, controllers



Ajout de `InscriptionServiceBase`, des mappers, des factories de messages, du `GlobalExceptionFilter` et des controllers.



\*\*Inscription d'un vendeur\*\* : `POST /api/vendeurs` → \*\*201 Created\*\*



```json

{

&#x20; "nomBoutique": "Chez Sara",

&#x20; "id": 1,

&#x20; "nom": "Benali",

&#x20; "prenom": "Sara",

&#x20; "email": "sara@mail.com",

&#x20; "statut": "EnAttente",

&#x20; "dateInscription": "2026-11-02T22:34:52.0524991Z"

}

```



\*\*Preuve du Singleton\*\* : le message de création n'apparaît qu'une fois et l'identifiant d'instance reste identique pour des requêtes différentes.



```

EmailSender créé (instance 80462994-0bc9-4639-a5ef-0995623bc99a)

\[Instance 80462994-...] Email simulé vers sara@mail.com | Sujet : Bienvenue sur notre plateforme de vente

\[Instance 80462994-...] Email simulé vers youssef@mail.com | Sujet : Votre inscription est confirmée

```



\*\*Même email une deuxième fois\*\* : \*\*409 Conflict\*\*



```json

{

&#x20; "title": "Conflit",

&#x20; "status": 409,

&#x20; "detail": "L'email sara@mail.com est déjà utilisé."

}

```



\*\*Email invalide\*\* : \*\*400 Bad Request\*\*



```json

{

&#x20; "title": "One or more validation errors occurred.",

&#x20; "status": 400,

&#x20; "errors": { "Email": \["The Email field is not a valid e-mail address."] }

}

```



\*\*Vendeur inexistant\*\* (`GET /api/vendeurs/999`) : \*\*404 Not Found\*\*



```json

{

&#x20; "title": "Ressource introuvable",

&#x20; "status": 404,

&#x20; "detail": "Vendeur avec l'identifiant 999 introuvable."

}

```



\### Étape 4 : Strategy, Decorators, Proxy, Adapter



Ajout de la chaîne d'envoi et de trois paramètres dans `SmtpOptions`. Chaque test a modifié `appsettings.json` puis relancé l'API.



\*\*Chaîne normale\*\* (`Enabled: false`) : 201, avec en console



```

Envoi d'un email à chaine@mail.com...

\[Instance c9dad8d0-...] Email simulé vers chaine@mail.com | Sujet : Bienvenue sur notre plateforme de vente

Email traité pour chaine@mail.com en 13 ms

```



\*\*Adapter\*\* (`"Provider": "External"`) : 201, avec en console



```

Envoi d'un email à adapter@mail.com...

\[API externe] Message « Bienvenue sur notre plateforme de vente » posté pour adapter@mail.com

Email traité pour adapter@mail.com en 6 ms

```



\*\*Retry\*\* (`"Enabled": true`, `"Host": "localhost"`, `"Port": 2525`, aucun serveur à l'écoute) : 201 après environ 19 secondes



```

Tentative 1/3 échouée pour retry@mail.com, nouvel essai dans 500 ms

Tentative 2/3 échouée pour retry@mail.com, nouvel essai dans 1000 ms

Échec de l'envoi à retry@mail.com après 14065 ms

Inscription enregistrée mais email non envoyé pour retry@mail.com

```



\*\*Proxy\*\* (`"MaxEmailsPerMinute": 2`, trois inscriptions de suite) : 201 pour les trois, mais la troisième est refusée par le Proxy, sans aucune ligne « Tentative »



```

Limite de 2 emails/minute atteinte, envoi refusé pour limite3@mail.com

Inscription enregistrée mais email non envoyé pour limite3@mail.com

```



\### Étape 5 : Observer et State



Le service ne connaît plus l'envoi d'email : il publie `UtilisateurInscritEvent`, et deux abonnés réagissent. `NotifierAsync` est remplacée par la publication de l'événement.



\*\*Fonctionnement normal\*\* : 201, avec en console



```

Envoi d'un email à observer1@mail.com...

\[Instance 55caadfd-...] Email simulé vers observer1@mail.com | Sujet : Bienvenue sur notre plateforme de vente

Email traité pour observer1@mail.com en 11 ms

Nouvelle inscription : Vendeur #8 (observer1@mail.com)

```



\*\*Un abonné qui échoue ne bloque rien\*\* (`"Enabled": true`, port 2525, `"MaxRetryAttempts": 1`) : 201, et l'abonné du journal s'exécute malgré l'échec de l'email



```

Échec de l'envoi à observer2@mail.com après 4217 ms

L'abonné EnvoyerConfirmationHandler`1 a échoué pour l'événement UtilisateurInscritEvent`1

Nouvelle inscription : Vendeur #9 (observer2@mail.com)

```



\*\*State\*\*, séquence testée sur le vendeur 8 :



| Requête | Résultat |

|---|---|

| `POST /api/vendeurs/8/confirmer` | 200, `"statut": "Confirmee"` |

| `POST /api/vendeurs/8/confirmer` (2e fois) | 409, `Impossible d'exécuter l'action « Confirmer » depuis l'état Confirmee.` |

| `POST /api/vendeurs/8/suspendre` | 200, `"statut": "Suspendue"` |

| `POST /api/vendeurs/8/confirmer` | 409, `... depuis l'état Suspendue.` |

| `POST /api/vendeurs/8/reactiver` | 200, `"statut": "Confirmee"` |

| `POST /api/vendeurs/9/reactiver` (vendeur `EnAttente`) | 409, `... depuis l'état EnAttente.` |

| `POST /api/vendeurs/999/confirmer` | 404 |



\### Étape 6 : Command, Mediator, Facade



Les controllers d'inscription passent maintenant par `IInscriptionFacade`, qui envoie une commande au Mediator.



\*\*Inscription\*\* : 201, avec en console



```

Commande InscrireVendeurCommand transmise à InscrireVendeurCommandHandler

```



\*\*Doublon\*\* : toujours \*\*409\*\* (et pas 500), donc les erreurs du service traversent le Mediator jusqu'au filtre global. Un email invalide reste en \*\*400\*\*, sans aucune ligne « Commande », car la validation s'arrête avant le Mediator.



\### Étape 7 : Abstract Factory



Ajout de la propriété `Langue` sur les utilisateurs, des kits français et anglais et d'une migration `AjoutLangue`. Les anciens enregistrements ont reçu `Fr` grâce à la valeur par défaut.



\*\*Vendeur sans langue\*\* : 201 avec `"langue": "Fr"`



```

Sujet : Bienvenue sur notre plateforme de vente | Corps : Bonjour Francais Kit, votre boutique « Boutique Kit 1 » est enregistrée depuis le mardi 3 novembre 2026. Vous pouvez dès maintenant publier vos produits.

```



\*\*Acheteur avec `"langue": "En"`\*\* : 201 avec `"langue": "En"`



```

Sujet : Your registration is confirmed | Corps : Hello English Kit, your buyer account was created on Tuesday, November 3, 2026. Delivery address on file: 10 Main Street, Oujda.

```



\*\*Langue inconnue\*\* (`"langue": "De"`) : \*\*400 Bad Request\*\*, la valeur n'existe pas dans l'enum.



\## Points d'attention



\- \*\*Sur-ingénierie assumée\*\* : pour une seule fonctionnalité, Command, Mediator et Facade empilés sont lourds. C'est volontaire pour l'exercice, dans un vrai code on n'en garderait qu'un.

\- \*\*Envoi synchrone\*\* : l'API attend la fin de l'envoi avant de répondre (jusqu'à environ 19 secondes avec un serveur d'email en panne). L'Observer découple le code mais n'accélère pas la réponse.

\- \*\*Strategy SMS\*\* : elle est enregistrée mais seul le canal email est appelé, c'est un exemple d'interchangeabilité.

\- \*\*State\*\* : après `Reactiver`, on revient à `Confirmee`, même si l'utilisateur n'avait jamais été confirmé avant sa suspension.

\- \*\*Mediator\*\* : l'appel de `HandleAsync` utilise `dynamic` pour choisir le bon handler à l'exécution.



\## Pistes pour aller plus loin



\- Envoyer les emails en arrière-plan avec une file d'attente et un `BackgroundService`.

\- Ajouter des tests unitaires (le couplage faible les facilite).

\- Utiliser une vraie bibliothèque Mediator et un vrai fournisseur d'email.

\- Ajouter l'authentification et une pagination sur les listes.

