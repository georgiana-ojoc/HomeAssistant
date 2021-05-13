# HomeAssistant
## Functional requirements
 - manage (add, view, modify and remove) smart devices, grouped by houses and rooms  
 - remotely control smart devices over the same Wi-Fi network  
	- adjust lights color and intensity  
	- lock and unlock doors  
	- set thermostats temperature  
 - obtain information from smart devices over the same Wi-Fi network (lights color, lights intensity, doors status, thermostats temperature)  
 - schedule custom actions involving different smart devices  
	 - sleep mode example: turn off the lights, lock the doors  
## Nonfunctional requirements
- can be used by thousands of customers (small families)  
- each customer can add to his account up to 5 houses, 20 rooms per house and 10 devices by type per room  
- each customer can add up to 20 schedules and 10 device commands by type per schedule
## Architectural decisions
1. [Diagrams](https://github.com/georgiana-ojoc/HomeAssistant/tree/documentation/Diagrams/1)
2. [Diagrams](https://github.com/georgiana-ojoc/HomeAssistant/tree/documentation/Diagrams/2)
	- modified database from SQL Server to Azure SQL
2. [Diagrams](https://github.com/georgiana-ojoc/HomeAssistant/tree/documentation/Diagrams/3)
	- modified web application from SPA to MPA
## AI/ML
 - personalized schedules, based on collected information
## CI/CD
 - Azure
