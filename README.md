# HomeAssistant
## Functional requirements
 - manage (add, view, modify and remove) smart devices, grouped by houses and rooms  
 - remotely control smart devices over the same Wi-Fi network  
	- adjust lights intensity  
	- lock and unlock doors  
	- change angle of surveillance cameras  
	- set thermostats temperature  
 - obtain information from smart devices over the same Wi-Fi network (lights intensity, doors status, cameras footage, thermostats temperature)  
 - schedule custom actions involving different smart devices  
	 - sleep mode example: turn off the lights, lock the doors  
	 - motion detection example: send alarm and start recording
## Nonfunctional requirements
- can be used by thousands of customers (small families)  
- each customer can add up to 5 houses, 50 rooms and 150 smart devices in his account
## Architectural decisions
1. [Diagrams](https://github.com/georgiana-ojoc/HomeAssistant/tree/documentation/Diagrams/1)
## AI/ML
 - personalized schedules, based on collected information
## CI/CD
 - Heroku with Docker
