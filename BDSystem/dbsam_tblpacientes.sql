CREATE DATABASE  IF NOT EXISTS `dbsam`;
USE `dbsam`;


DROP TABLE IF EXISTS `tblpacientes`;

CREATE TABLE `tblpacientes` (
  `CPF` varchar(14) NOT NULL,
  `nomeCompleto` varchar(255) DEFAULT NULL,
  `caminhoImagem` varchar(20) DEFAULT NULL,
  `dataNascimento` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`CPF`)
);

LOCK TABLES `tblpacientes` WRITE;
UNLOCK TABLES;