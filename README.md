# Trends3-Group2

Wij hebben een lokaal docker aan gemaakt. Dus voor dat jullie de code laat runnen. 
Moeten jullie eerst een docker lokaal aanmaken wij hebben onze container gemaakt met volgende commando 
"docker run -d --hostname my-rabbit --name ecomm-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management".
Eens dat jullie de container hebben aangemaakt, dan kunnen jullie de code van Github clone.
Daarna moeten jullie in de nuget RabbitMQ.Client instellen.
Nu zijn jullie klaar om deze code te laten runnen en te testen.
Testen kan via het volgende link: http://localhost:15672/#/
