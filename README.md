# Trends3-Group2

Wij hebben een lokaal docker aan gemaakt. Dus voor dat jullie de code laat runnen. 
Moeten jullie eerst een docker lokaal aanmaken. 
Wij hebben ons container gemaakt met volgende commando: 
"docker run -d --hostname my-rabbit --name ecomm-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management".
Eens dat jullie de container hebben aangemaakt, dan kunnen jullie de code van Github clone.
Indien het juiste mapstructuur niet juist is in de code (gelieve deze aan te passen naar nodig waar jullie deze hebben opgeslagen).
Hierbij zit ook het andere programma OutQueue bij de zip bestanden. Start deze programma nadat de IN Queue programma is gestart van deze GitHub.
Daarna moeten jullie de nuget RabbitMQ.Client instellen.
Nu zijn jullie klaar om deze code te laten runnen en te testen.
Testen kan via het volgende link: http://localhost:15672/#/
