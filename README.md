# Rapport: Een AI-Agent trainen voor het ophalen en wegbrengen van objecten in een virtuele omgeving

## Inleiding
Ik heb voor de eerste keer een AI getrained en ik weet nu de belangrijkste methoden en hoe ik deze gebruik. 
In deze specifieke oefening heb ik een AI getrained om eerst een object te halen en nadien dit object naar een zone te brengen. 
Deze oefening is bedoeld voor mijzelf en mijn docent. Het doel van deze oefening was dus om de basis van het AI trainen onder de knie krijgen.

## Methoden
* ## Primaire componenten
  * Behaviour parameters: Dit is eigenlijk het brein van de Ai.
    hier kan je de Space size ingeven. Dit is belangrijk voor de AI omdat hij zo weet hoeveel inputs hij krijgt.
    Daarnaast wordt het ook gebruikt om een bestaand AI-model te gebruiken.
  * Agent: Dit is het lichaam van de AI en dus het script dat ok de Agent zit.
    In dit script worden obervations, actions en rewards gegeven. Ook zeggen we wanneer een episode start en moet eindigen. 
* ## Override methods
  * **`OnEpisodeBegin()`**: Deze methode zet alles klaar om te beginnen aan de epsiode. In deze oefening is dit om de boolean `hasTakenRed` naar false te zetten.
  * **`CollectObservations()`**: Zeer belangrijke methode want anders is de AI blind. In deze oefening is deze methode gebruikt om te zeggen of hij naar het target
    moet zoeken of dat hij het target al heeft en naar het doel moet brengen.
  * **`OnActionReceived()`**: Hier wordt gebruik gemaakt van getallen zodat de AI weet of hij goed bezig is of niet. In deze methode worden ook de rewards toegewezen
    voor de acties die de AI doet. In deze oefening krijgt de AI een reward als hij het target heeft gevonden en ook als hij het target naar de zone brengt.
    Hij krijgt een straf als hij van het platform valt. Hier wordt ook de episode beïndigd.
  * **`Heuristic()`**: Deze methode zorgt ervoor dat ik zelf de AI kan besturen. Ik kan de controle over de AI dus zelf grijpen.

 ## Resultaten
 In het begin van het trainen viel de AI meestal van het platform of wist hij niet wat hij moest doen. De AI draaide vaak rondjes. 
 Naar het einde toe wist de AI meer wat hij moest doen en bleef hij op het platform, pakte hij het target en bracht hij het naar het doel.

 ## Conclusie
 De AI wist op het einde dat hij eerst het target moest gaan halen en nadien het target naar het doel moest brengen. Dit komt omdat hij hier rewards voor kreeg. 

## Referenties
/
 
