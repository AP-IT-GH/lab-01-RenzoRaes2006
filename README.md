# Rapport: Een AI-Agent trainen voor het ophalen en wegbrengen van objecten in een virtuele omgeving

## Inleiding
Dit rapport behandelt de belangrijkste methoden en de toepassing daarvan bij het trainen van een AI-agent. De focus van de oefening ligt op het aanleren van een specifieke taakvolgorde: het verzamelen van een object gevolgd door het transporteren van dit object naar een aangewezen zone. Het doel van dit onderzoek is het beheersen van de basisprincipes van AI-training binnen een gesimuleerde omgeving.
## Methoden
* ## Primaire componenten
  * Behaviour parameters: Dit component fungeert als het centrale brein van de agent. Hierin wordt de Space Size gedefinieerd, wat essentieel is zodat de agent het aantal inputsignalen herkent. Daarnaast faciliteert dit onderdeel het laden van bestaande AI-modellen.
  * Agent: Dit betreft de fysieke entiteit en het bijbehorende script. Binnen dit script worden de observaties, acties en beloningen beheerd. Tevens wordt hierin bepaald wanneer een episode start en eindigt.
* ## Override methods
  * **`OnEpisodeBegin()`**: Deze methode bereidt de omgeving voor op een nieuwe episode. In deze oefening wordt de status van het verzamelde object (boolean hasTakenRed) gereset naar de beginwaarde.
  * **`CollectObservations()`**: Deze methode voorziet de agent van informatie over de omgeving. De functie wordt aangewend om aan te geven of de agent moet zoeken naar het target, dan wel het reeds verzamelde target naar het doel moet transporteren.
  * **`OnActionReceived()`**: In deze methode worden getallen verwerkt die de voortgang van de agent evalueren. Beloningen worden toegewezen op basis van de uitgevoerde acties, zoals het vinden van het target of het bereiken van de doelzone. Bij het verlaten van het platform volgt een straf en wordt de episode beëindigd.
    voor de acties die de AI doet. In deze oefening krijgt de AI een reward als hij het target heeft gevonden en ook als hij het target naar de zone brengt.
    Hij krijgt een straf als hij van het platform valt. Hier wordt ook de episode beïndigd.
  * **`Heuristic()`**: Deze methode maakt handmatige besturing van de agent mogelijk, waardoor directe controle over de acties kan worden uitgeoefend voor testdoeleinden.

 ## Resultaten
 Bij de start van het trainingsproces werd geobserveerd dat de agent regelmatig het platform verliet of cirkelvormige bewegingen maakte zonder duidelijke richting. Naarmate de training vorderde, stabiliseerde het gedrag. De agent bleef op het platform, verzamelde het target en bracht dit naar de doelzone.

 ## Conclusie
 De agent vertoont het vermogen om de volgorde van de taak — eerst het target ophalen en vervolgens naar het doel brengen — te voltooien. De interpretatie van dit gedrag wijst op een succesvolle koppeling tussen de acties en de bijbehorende beloningen (rewards).
![Graph](GraphExerciseOne)

## Referenties
Unity Technologies (2024). Agents - Unity ML-Agents Documentation (Version 4.0). Geraadpleegd op 16 maart 2026, van https://docs.unity3d.com/Packages/com.unity.ml-agents@4.0/manual/Learning-Environment-Design-Agents.html
 
