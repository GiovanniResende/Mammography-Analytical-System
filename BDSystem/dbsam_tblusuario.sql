CREATE DATABASE  IF NOT EXISTS `dbsam`;
USE `dbsam`;

DROP TABLE IF EXISTS `tblusuario`;

CREATE TABLE `tblusuario` (
  `id` int NOT NULL AUTO_INCREMENT,
  `usuario` varchar(45) NOT NULL,
  `senha` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
);

LOCK TABLES `tblusuario` WRITE;
INSERT INTO `tblusuario` VALUES (1,'admin','123456');
UNLOCK TABLES;