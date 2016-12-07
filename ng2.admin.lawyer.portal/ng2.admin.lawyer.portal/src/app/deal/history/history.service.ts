import { Injectable } from '@angular/core';

@Injectable()
export class DealHistoryService {

    dealHistoryData = [
        {
            "DealHistoryID": "182976",
            "DealID": "27498",
            "Activity": "A new deal has been submitted by the Lender.",
            "ActivityFrench": "Une nouvelle transaction a �t� soumise par le Pr�teur.",
            "LogDate": "2015-06-16 13:42:32.900000000",
            "UserID": "SYSTEM",
            "UserType": "LENDER",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "182984",
            "DealID": "27498",
            "Activity": "LLC deal has been accepted.",
            "ActivityFrench": "La transaction LLC a �t� accept�e.",
            "LogDate": "2015-06-16 13:46:47.423000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183039",
            "DealID": "27498",
            "Activity": "Closing date changed from Sep 28/2015 to Jun 24/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 28 sept. 2015 � 24 juin 2015.",
            "LogDate": "2015-06-19 09:54:15.107000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183040",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 10:15:07.217000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183041",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 24/2015 to Jun 25/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 24 juin 2015 � 25 juin 2015.",
            "LogDate": "2015-06-19 11:13:48.900000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183042",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 11:13:53.510000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183043",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 25/2015 to Jun 26/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 25 juin 2015 � 26 juin 2015.",
            "LogDate": "2015-06-19 11:16:42.157000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183044",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 11:16:45.970000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183045",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 26/2015 to Jun 29/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 26 juin 2015 � 29 juin 2015.",
            "LogDate": "2015-06-19 11:17:27.237000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183046",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 11:17:32.110000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183047",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 29/2015 to Jun 30/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 29 juin 2015 � 30 juin 2015.",
            "LogDate": "2015-06-19 11:24:35.283000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183048",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 11:24:39.923000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183049",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 30/2015 to Jun 24/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 30 juin 2015 � 24 juin 2015.",
            "LogDate": "2015-06-19 11:28:10.213000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183050",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 11:28:12.900000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183051",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 24/2015 to Jun 30/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 24 juin 2015 � 30 juin 2015.",
            "LogDate": "2015-06-19 11:41:46.650000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183052",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 11:41:54.057000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183053",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 30/2015 to Jun 24/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 30 juin 2015 � 24 juin 2015.",
            "LogDate": "2015-06-19 11:42:57.340000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183054",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 11:43:35.093000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183055",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 24/2015 to Jun 30/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 24 juin 2015 � 30 juin 2015.",
            "LogDate": "2015-06-19 12:05:43.503000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183056",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 12:05:47.190000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183057",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 30/2015 to Jun 24/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 30 juin 2015 � 24 juin 2015.",
            "LogDate": "2015-06-19 12:07:44.930000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183058",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 12:07:51.900000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183059",
            "DealID": "27498",
            "Activity": "Closing date changed from Jun 24/2015 to Jun 25/2015.",
            "ActivityFrench": "Date de cl�ture modifi�e mis � jour de 24 juin 2015 � 25 juin 2015.",
            "LogDate": "2015-06-19 12:12:06.363000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183060",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 12:12:12.377000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183061",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 9052871000 to 647620736431.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 9052871000 � 647620736431.",
            "LogDate": "2015-06-19 15:59:06.400000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183062",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Janet Organics Inc Lee to Update Company Name31.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Janet Organics Inc Lee � Update Company Name31.",
            "LogDate": "2015-06-19 15:59:06.417000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183063",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Desauls to Update First Name31.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Desauls � Update First Name31.",
            "LogDate": "2015-06-19 15:59:06.417000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183064",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Jacky to Update Last Name31.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Jacky � Update Last Name31.",
            "LogDate": "2015-06-19 15:59:06.430000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183065",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 9052871000 to 905647908731.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 9052871000 � 905647908731.",
            "LogDate": "2015-06-19 15:59:06.853000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183066",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Hamilton to Brampton31.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Hamilton � Brampton31.",
            "LogDate": "2015-06-19 15:59:06.870000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183067",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada to Canada31.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada � Canada31.",
            "LogDate": "2015-06-19 15:59:06.870000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183068",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 9052871000 to 416816447031.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 9052871000 � 416816447031.",
            "LogDate": "2015-06-19 15:59:06.870000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183069",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Donald to Update Person Last Name31.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Donald � Update Person Last Name31.",
            "LogDate": "2015-06-19 15:59:06.883000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183070",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from GlenErin Drive to Earnccliffe31.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de GlenErin Drive � Earnccliffe31.",
            "LogDate": "2015-06-19 15:59:06.900000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183071",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 24 to 1331.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 24 � 1331.",
            "LogDate": "2015-06-19 15:59:06.900000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183072",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 8 to 1631.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 8 � 1631.",
            "LogDate": "2015-06-19 15:59:06.917000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183073",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 15:59:42.903000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183075",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 647620736431 to 647620736416.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 647620736431 � 647620736416.",
            "LogDate": "2015-06-19 16:02:37.300000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183076",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name31 to Update Company Name16.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name31 � Update Company Name16.",
            "LogDate": "2015-06-19 16:02:37.313000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183077",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name31 to Update First Name16.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name31 � Update First Name16.",
            "LogDate": "2015-06-19 16:02:37.313000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183078",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name31 to Update Last Name16.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name31 � Update Last Name16.",
            "LogDate": "2015-06-19 16:02:37.313000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183079",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 905647908731 to 905647908716.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 905647908731 � 905647908716.",
            "LogDate": "2015-06-19 16:02:37.377000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183080",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton31 to Brampton16.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton31 � Brampton16.",
            "LogDate": "2015-06-19 16:02:37.393000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183081",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada31 to Canada16.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada31 � Canada16.",
            "LogDate": "2015-06-19 16:02:37.393000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183082",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 416816447031 to 416816447016.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 416816447031 � 416816447016.",
            "LogDate": "2015-06-19 16:02:37.410000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183083",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name31 to Update Person Last Name16.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name31 � Update Person Last Name16.",
            "LogDate": "2015-06-19 16:02:37.410000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183084",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe31 to Earnccliffe16.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe31 � Earnccliffe16.",
            "LogDate": "2015-06-19 16:02:37.410000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183085",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 1331 to 1316.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 1331 � 1316.",
            "LogDate": "2015-06-19 16:02:37.423000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183086",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 1631 to 1616.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 1631 � 1616.",
            "LogDate": "2015-06-19 16:02:37.423000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183087",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 16:02:44.220000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183088",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 647620736416 to 647620736428.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 647620736416 � 647620736428.",
            "LogDate": "2015-06-19 16:06:37.760000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183089",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name16 to Update Company Name28.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name16 � Update Company Name28.",
            "LogDate": "2015-06-19 16:06:37.760000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183090",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name16 to Update First Name28.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name16 � Update First Name28.",
            "LogDate": "2015-06-19 16:06:37.777000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183091",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name16 to Update Last Name28.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name16 � Update Last Name28.",
            "LogDate": "2015-06-19 16:06:37.777000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183092",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 905647908716 to 905647908728.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 905647908716 � 905647908728.",
            "LogDate": "2015-06-19 16:06:37.840000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183093",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton16 to Brampton28.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton16 � Brampton28.",
            "LogDate": "2015-06-19 16:06:37.840000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183094",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada16 to Canada28.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada16 � Canada28.",
            "LogDate": "2015-06-19 16:06:37.840000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183095",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 416816447016 to 416816447028.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 416816447016 � 416816447028.",
            "LogDate": "2015-06-19 16:06:37.857000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183096",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name16 to Update Person Last Name28.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name16 � Update Person Last Name28.",
            "LogDate": "2015-06-19 16:06:37.857000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183097",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe16 to Earnccliffe28.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe16 � Earnccliffe28.",
            "LogDate": "2015-06-19 16:06:37.870000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183098",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 1316 to 1328.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 1316 � 1328.",
            "LogDate": "2015-06-19 16:06:37.870000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183099",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 1616 to 1628.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 1616 � 1628.",
            "LogDate": "2015-06-19 16:06:37.870000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183100",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 16:06:44.747000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183101",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 647620736428 to 647620736451.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 647620736428 � 647620736451.",
            "LogDate": "2015-06-19 16:08:59.940000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183102",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name28 to Update Company Name51.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name28 � Update Company Name51.",
            "LogDate": "2015-06-19 16:08:59.940000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183103",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name28 to Update First Name51.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name28 � Update First Name51.",
            "LogDate": "2015-06-19 16:08:59.953000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183104",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name28 to Update Last Name51.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name28 � Update Last Name51.",
            "LogDate": "2015-06-19 16:08:59.953000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183105",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 905647908728 to 905647908751.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 905647908728 � 905647908751.",
            "LogDate": "2015-06-19 16:09:00.017000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183106",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton28 to Brampton51.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton28 � Brampton51.",
            "LogDate": "2015-06-19 16:09:00.017000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183107",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada28 to Canada51.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada28 � Canada51.",
            "LogDate": "2015-06-19 16:09:00.017000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183108",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 416816447028 to 416816447051.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 416816447028 � 416816447051.",
            "LogDate": "2015-06-19 16:09:00.033000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183109",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name28 to Update Person Last Name51.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name28 � Update Person Last Name51.",
            "LogDate": "2015-06-19 16:09:00.033000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183110",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe28 to Earnccliffe51.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe28 � Earnccliffe51.",
            "LogDate": "2015-06-19 16:09:00.033000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183111",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 1328 to 1351.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 1328 � 1351.",
            "LogDate": "2015-06-19 16:09:00.047000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183112",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 1628 to 1651.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 1628 � 1651.",
            "LogDate": "2015-06-19 16:09:00.047000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183113",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-19 16:09:07.050000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183118",
            "DealID": "27498",
            "Activity": "Request for Funds follow up notification sent to Lawyer.",
            "ActivityFrench": "L'avis de suivi pour la Demande de fonds a �t� envoy� � l'avocat/au notaire.",
            "LogDate": "2015-06-19 21:00:10.310000000",
            "UserID": "SYSTEM",
            "UserType": "SYSTEM",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183206",
            "DealID": "27498",
            "Activity": "Guarantor/Covenantor Company Name updated from ABC company Inc Ltd to Update Guarantor Company Name1.",
            "ActivityFrench": " Garant/Caution Nom de la soci�t�  mis � jour de ABC company Inc Ltd � Update Guarantor Company Name1.",
            "LogDate": "2015-06-23 10:03:04.987000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183207",
            "DealID": "27498",
            "Activity": "Guarantor/Covenantor Contact First Name updated from Tom to Update Contact FN1.",
            "ActivityFrench": " Garant/Caution Pr�nom de la personne-ressource  mis � jour de Tom � Update Contact FN1.",
            "LogDate": "2015-06-23 10:03:05.017000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183208",
            "DealID": "27498",
            "Activity": "Guarantor/Covenantor Contact Last Name updated from James to Update Contact LN1.",
            "ActivityFrench": " Garant/Caution Nom de la personne-ressource  mis � jour de James � Update Contact LN1.",
            "LogDate": "2015-06-23 10:03:05.033000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183209",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-23 10:04:29.223000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183210",
            "DealID": "27498",
            "Activity": "Property (Unit Number) updated from 49709 to 18.",
            "ActivityFrench": "Propri�t� (adresse civique) mise-�-jour mis � jour de 49709 � 18.",
            "LogDate": "2015-06-23 10:52:00.067000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183211",
            "DealID": "27498",
            "Activity": "Property (Street Name) updated from Crestlawn Court to Update Street Name.",
            "ActivityFrench": "Propri�t� (nom de la rue)  mis � jour de Crestlawn Court � Update Street Name.",
            "LogDate": "2015-06-23 10:52:00.083000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183212",
            "DealID": "27498",
            "Activity": "Property (Street Name) updated from Property street to Update Street Name2.",
            "ActivityFrench": "Propri�t� (nom de la rue)  mis � jour de Property street � Update Street Name2.",
            "LogDate": "2015-06-23 10:52:00.083000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183213",
            "DealID": "27498",
            "Activity": "Property (City) updated from Malton to Mississauga.",
            "ActivityFrench": "Propri�t� (ville) mise-�-jour mis � jour de Malton � Mississauga.",
            "LogDate": "2015-06-23 10:52:00.083000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183214",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-23 10:52:07.553000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183216",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 647620736451 to 647620736418.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 647620736451 � 647620736418.",
            "LogDate": "2015-06-23 15:27:18.447000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183217",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name51 to Update Company Name18.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name51 � Update Company Name18.",
            "LogDate": "2015-06-23 15:27:18.463000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183218",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name51 to Update First Name18.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name51 � Update First Name18.",
            "LogDate": "2015-06-23 15:27:18.463000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183219",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name51 to Update Last Name18.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name51 � Update Last Name18.",
            "LogDate": "2015-06-23 15:27:18.480000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183220",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 905647908751 to 905647908718.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 905647908751 � 905647908718.",
            "LogDate": "2015-06-23 15:27:18.870000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183221",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton51 to Brampton18.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton51 � Brampton18.",
            "LogDate": "2015-06-23 15:27:18.887000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183222",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada51 to Canada18.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada51 � Canada18.",
            "LogDate": "2015-06-23 15:27:18.887000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183223",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 416816447051 to 416816447018.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 416816447051 � 416816447018.",
            "LogDate": "2015-06-23 15:27:18.900000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183224",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name51 to Update Person Last Name18.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name51 � Update Person Last Name18.",
            "LogDate": "2015-06-23 15:27:18.900000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183225",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe51 to Earnccliffe18.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe51 � Earnccliffe18.",
            "LogDate": "2015-06-23 15:27:18.917000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183226",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 1351 to 1318.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 1351 � 1318.",
            "LogDate": "2015-06-23 15:27:18.917000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183227",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 1651 to 1618.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 1651 � 1618.",
            "LogDate": "2015-06-23 15:27:18.933000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183228",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-23 15:31:49.863000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183229",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 647620736418 to 64762073644.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 647620736418 � 64762073644.",
            "LogDate": "2015-06-23 15:33:18.447000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183230",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name18 to Update Company Name4.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name18 � Update Company Name4.",
            "LogDate": "2015-06-23 15:33:18.460000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183231",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name18 to Update First Name4.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name18 � Update First Name4.",
            "LogDate": "2015-06-23 15:33:18.493000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183232",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name18 to Update Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name18 � Update Last Name4.",
            "LogDate": "2015-06-23 15:33:18.510000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183233",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 905647908718 to 90564790874.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 905647908718 � 90564790874.",
            "LogDate": "2015-06-23 15:33:18.557000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183234",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton18 to Brampton4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton18 � Brampton4.",
            "LogDate": "2015-06-23 15:33:18.557000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183235",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada18 to Canada4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada18 � Canada4.",
            "LogDate": "2015-06-23 15:33:18.570000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183236",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 416816447018 to 41681644704.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 416816447018 � 41681644704.",
            "LogDate": "2015-06-23 15:33:18.570000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183237",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name18 to Update Person Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name18 � Update Person Last Name4.",
            "LogDate": "2015-06-23 15:33:18.587000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183238",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe18 to Earnccliffe4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe18 � Earnccliffe4.",
            "LogDate": "2015-06-23 15:33:18.587000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183239",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 1318 to 134.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 1318 � 134.",
            "LogDate": "2015-06-23 15:33:18.587000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183240",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 1618 to 164.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 1618 � 164.",
            "LogDate": "2015-06-23 15:33:18.603000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183241",
            "DealID": "27498",
            "Activity": "The Lawyer has submitted changes.",
            "ActivityFrench": "L'avocat/le notaire a soumis les modifications.",
            "LogDate": "2015-06-23 15:36:16.017000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183242",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 64762073644 to 64762073645.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 64762073644 � 64762073645.",
            "LogDate": "2015-06-23 15:44:26.533000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183243",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name4 to Update Company Name5.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name4 � Update Company Name5.",
            "LogDate": "2015-06-23 15:44:26.533000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183244",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name4 to Update First Name5.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name4 � Update First Name5.",
            "LogDate": "2015-06-23 15:44:26.550000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183245",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name4 to Update Last Name5.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name4 � Update Last Name5.",
            "LogDate": "2015-06-23 15:44:26.550000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183246",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 90564790874 to 90564790875.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 90564790874 � 90564790875.",
            "LogDate": "2015-06-23 15:44:26.627000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183247",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton4 to Brampton5.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton4 � Brampton5.",
            "LogDate": "2015-06-23 15:44:26.643000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183248",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada4 to Canada5.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada4 � Canada5.",
            "LogDate": "2015-06-23 15:44:26.643000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183249",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 41681644704 to 41681644705.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 41681644704 � 41681644705.",
            "LogDate": "2015-06-23 15:44:26.660000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183250",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name4 to Update Person Last Name5.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name4 � Update Person Last Name5.",
            "LogDate": "2015-06-23 15:44:26.660000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183251",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe4 to Earnccliffe5.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe4 � Earnccliffe5.",
            "LogDate": "2015-06-23 15:44:26.660000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183252",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 134 to 135.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 134 � 135.",
            "LogDate": "2015-06-23 15:44:26.673000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183253",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 164 to 165.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 164 � 165.",
            "LogDate": "2015-06-23 15:44:26.673000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183254",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 64762073645 to 64762073644.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 64762073645 � 64762073644.",
            "LogDate": "2015-06-23 15:46:10.787000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183255",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name5 to Update Company Name4.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name5 � Update Company Name4.",
            "LogDate": "2015-06-23 15:46:10.787000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183256",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name5 to Update First Name4.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name5 � Update First Name4.",
            "LogDate": "2015-06-23 15:46:10.803000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183257",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name5 to Update Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name5 � Update Last Name4.",
            "LogDate": "2015-06-23 15:46:10.803000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183258",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 90564790875 to 90564790874.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 90564790875 � 90564790874.",
            "LogDate": "2015-06-23 15:46:10.883000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183259",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton5 to Brampton4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton5 � Brampton4.",
            "LogDate": "2015-06-23 15:46:10.883000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183260",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada5 to Canada4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada5 � Canada4.",
            "LogDate": "2015-06-23 15:46:10.897000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183261",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 41681644705 to 41681644704.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 41681644705 � 41681644704.",
            "LogDate": "2015-06-23 15:46:10.897000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183262",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name5 to Update Person Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name5 � Update Person Last Name4.",
            "LogDate": "2015-06-23 15:46:10.897000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183263",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe5 to Earnccliffe4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe5 � Earnccliffe4.",
            "LogDate": "2015-06-23 15:46:10.913000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183264",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 135 to 134.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 135 � 134.",
            "LogDate": "2015-06-23 15:46:10.913000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183265",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 165 to 164.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 165 � 164.",
            "LogDate": "2015-06-23 15:46:10.913000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183301",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 64762073644 to 647620736419.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 64762073644 � 647620736419.",
            "LogDate": "2015-06-24 07:32:21.960000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183302",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name4 to Update Company Name19.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name4 � Update Company Name19.",
            "LogDate": "2015-06-24 07:32:21.973000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183303",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name4 to Update First Name19.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name4 � Update First Name19.",
            "LogDate": "2015-06-24 07:32:21.973000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183304",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name4 to Update Last Name19.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name4 � Update Last Name19.",
            "LogDate": "2015-06-24 07:32:21.973000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183305",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 90564790874 to 905647908719.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 90564790874 � 905647908719.",
            "LogDate": "2015-06-24 07:32:22.583000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183306",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton4 to Brampton19.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton4 � Brampton19.",
            "LogDate": "2015-06-24 07:32:22.600000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183307",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada4 to Canada19.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada4 � Canada19.",
            "LogDate": "2015-06-24 07:32:22.600000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183308",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 41681644704 to 416816447019.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 41681644704 � 416816447019.",
            "LogDate": "2015-06-24 07:32:22.613000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183309",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name4 to Update Person Last Name19.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name4 � Update Person Last Name19.",
            "LogDate": "2015-06-24 07:32:22.613000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183310",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe4 to Earnccliffe19.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe4 � Earnccliffe19.",
            "LogDate": "2015-06-24 07:32:22.613000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183311",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 134 to 1319.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 134 � 1319.",
            "LogDate": "2015-06-24 07:32:22.630000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183312",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 164 to 1619.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 164 � 1619.",
            "LogDate": "2015-06-24 07:32:22.630000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183314",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 647620736419 to 64762073644.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 647620736419 � 64762073644.",
            "LogDate": "2015-06-24 07:35:29.123000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183315",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name19 to Update Company Name4.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name19 � Update Company Name4.",
            "LogDate": "2015-06-24 07:35:29.123000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183316",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name19 to Update First Name4.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name19 � Update First Name4.",
            "LogDate": "2015-06-24 07:35:29.137000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183317",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name19 to Update Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name19 � Update Last Name4.",
            "LogDate": "2015-06-24 07:35:29.137000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183318",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 905647908719 to 90564790874.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 905647908719 � 90564790874.",
            "LogDate": "2015-06-24 07:35:29.217000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183319",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton19 to Brampton4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton19 � Brampton4.",
            "LogDate": "2015-06-24 07:35:29.230000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183320",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada19 to Canada4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada19 � Canada4.",
            "LogDate": "2015-06-24 07:35:29.230000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183321",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 416816447019 to 41681644704.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 416816447019 � 41681644704.",
            "LogDate": "2015-06-24 07:35:29.230000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183322",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name19 to Update Person Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name19 � Update Person Last Name4.",
            "LogDate": "2015-06-24 07:35:29.247000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183323",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe19 to Earnccliffe4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe19 � Earnccliffe4.",
            "LogDate": "2015-06-24 07:35:29.247000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183324",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 1319 to 134.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 1319 � 134.",
            "LogDate": "2015-06-24 07:35:29.247000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183325",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 1619 to 164.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 1619 � 164.",
            "LogDate": "2015-06-24 07:35:29.263000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183326",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 64762073644 to 647620736435.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 64762073644 � 647620736435.",
            "LogDate": "2015-06-24 07:39:47.850000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183327",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name4 to Update Company Name35.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name4 � Update Company Name35.",
            "LogDate": "2015-06-24 07:39:47.867000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183328",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name4 to Update First Name35.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name4 � Update First Name35.",
            "LogDate": "2015-06-24 07:39:47.867000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183329",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name4 to Update Last Name35.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name4 � Update Last Name35.",
            "LogDate": "2015-06-24 07:39:47.883000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183330",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 90564790874 to 905647908735.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 90564790874 � 905647908735.",
            "LogDate": "2015-06-24 07:39:48.053000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183331",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton4 to Brampton35.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton4 � Brampton35.",
            "LogDate": "2015-06-24 07:39:48.070000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183332",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada4 to Canada35.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada4 � Canada35.",
            "LogDate": "2015-06-24 07:39:48.070000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183333",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 41681644704 to 416816447035.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 41681644704 � 416816447035.",
            "LogDate": "2015-06-24 07:39:48.083000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183334",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name4 to Update Person Last Name35.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name4 � Update Person Last Name35.",
            "LogDate": "2015-06-24 07:39:48.083000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183335",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe4 to Earnccliffe35.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe4 � Earnccliffe35.",
            "LogDate": "2015-06-24 07:39:48.083000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183336",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 134 to 1335.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 134 � 1335.",
            "LogDate": "2015-06-24 07:39:48.100000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183337",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 164 to 1635.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 164 � 1635.",
            "LogDate": "2015-06-24 07:39:48.100000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183338",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 647620736435 to 64762073644.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 647620736435 � 64762073644.",
            "LogDate": "2015-06-24 07:43:46.483000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183339",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name35 to Update Company Name4.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name35 � Update Company Name4.",
            "LogDate": "2015-06-24 07:43:46.483000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183340",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name35 to Update First Name4.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name35 � Update First Name4.",
            "LogDate": "2015-06-24 07:43:46.483000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183341",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name35 to Update Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name35 � Update Last Name4.",
            "LogDate": "2015-06-24 07:43:46.500000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183342",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 905647908735 to 90564790874.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 905647908735 � 90564790874.",
            "LogDate": "2015-06-24 07:43:46.580000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183343",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton35 to Brampton4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton35 � Brampton4.",
            "LogDate": "2015-06-24 07:43:46.593000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183344",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada35 to Canada4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada35 � Canada4.",
            "LogDate": "2015-06-24 07:43:46.593000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183345",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 416816447035 to 41681644704.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 416816447035 � 41681644704.",
            "LogDate": "2015-06-24 07:43:46.593000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183346",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name35 to Update Person Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name35 � Update Person Last Name4.",
            "LogDate": "2015-06-24 07:43:46.610000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183347",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe35 to Earnccliffe4.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe35 � Earnccliffe4.",
            "LogDate": "2015-06-24 07:43:46.610000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183348",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 1335 to 134.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 1335 � 134.",
            "LogDate": "2015-06-24 07:43:46.610000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183349",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 1635 to 164.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 1635 � 164.",
            "LogDate": "2015-06-24 07:43:46.627000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183677",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 64762073644 to 647620736446.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 64762073644 � 647620736446.",
            "LogDate": "2015-06-25 13:26:57.173000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183678",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name4 to Update Company Name46.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name4 � Update Company Name46.",
            "LogDate": "2015-06-25 13:26:57.203000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183679",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name4 to Update First Name46.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name4 � Update First Name46.",
            "LogDate": "2015-06-25 13:26:57.203000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183680",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name4 to Update Last Name46.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name4 � Update Last Name46.",
            "LogDate": "2015-06-25 13:26:57.203000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183681",
            "DealID": "27498",
            "Activity": "Mortgagor Business Phone updated from 90564790874 to 905647908746.",
            "ActivityFrench": " T�l�phone d'affaires du d�biteur hypoth�caire  mis � jour de 90564790874 � 905647908746.",
            "LogDate": "2015-06-25 13:26:57.830000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183682",
            "DealID": "27498",
            "Activity": "Mortgagor Address (City) updated from Brampton4 to Brampton46.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (ville)  mis � jour de Brampton4 � Brampton46.",
            "LogDate": "2015-06-25 13:26:57.830000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183683",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Country) updated from Canada4 to Canada46.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (pays)  mis � jour de Canada4 � Canada46.",
            "LogDate": "2015-06-25 13:26:57.843000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183684",
            "DealID": "27498",
            "Activity": "Mortgagor Home Phone updated from 41681644704 to 416816447046.",
            "ActivityFrench": " T�l�phone (maison) du d�biteur hypoth�caire  mis � jour de 41681644704 � 416816447046.",
            "LogDate": "2015-06-25 13:26:57.843000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183685",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Person Last Name4 to Update Person Last Name46.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Person Last Name4 � Update Person Last Name46.",
            "LogDate": "2015-06-25 13:26:57.843000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183686",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Name) updated from Earnccliffe4 to Earnccliffe46.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (nom de la rue)  mis � jour de Earnccliffe4 � Earnccliffe46.",
            "LogDate": "2015-06-25 13:26:57.860000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183687",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Street Number) updated from 134 to 1346.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (num�ro municipal)  mis � jour de 134 � 1346.",
            "LogDate": "2015-06-25 13:26:57.877000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183688",
            "DealID": "27498",
            "Activity": "Mortgagor Address (Unit Number) updated from 164 to 1646.",
            "ActivityFrench": " Adresse du d�biteur hypoth�caire (no d'appartement)  mis � jour de 164 � 1646.",
            "LogDate": "2015-06-25 13:26:57.890000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183689",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name46 to Update Company Name4.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name46 � Update Company Name4.",
            "LogDate": "2015-06-25 13:29:15.067000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183690",
            "DealID": "27498",
            "Activity": "Mortgagor First Name updated from Update First Name46 to Update First Name4.",
            "ActivityFrench": " Pr�nom du d�biteur hypoth�caire  mis � jour de Update First Name46 � Update First Name4.",
            "LogDate": "2015-06-25 13:29:15.083000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183691",
            "DealID": "27498",
            "Activity": "Mortgagor Last Name updated from Update Last Name46 to Update Last Name4.",
            "ActivityFrench": " Nom du d�biteur hypoth�caire  mis � jour de Update Last Name46 � Update Last Name4.",
            "LogDate": "2015-06-25 13:29:15.083000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "183824",
            "DealID": "27498",
            "Activity": "Mortgagor Company Name updated from Update Company Name4 to Update Company Name7.",
            "ActivityFrench": " Nom de l'entreprise du d�biteur hypoth�caire  mis � jour de Update Company Name4 � Update Company Name7.",
            "LogDate": "2015-06-26 10:12:16.307000000",
            "UserID": "wtestingfirm",
            "UserType": "Lawyer",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "191565",
            "DealID": "27498",
            "Activity": "Follow-Up for Final Report on Title Notice sent to Lawyer.",
            "ActivityFrench": "Avis de suivi sur l'envoi du rapport final sur les titres � l'avocat/notaire.",
            "LogDate": "2015-07-27 21:00:18.037000000",
            "UserID": "SYSTEM",
            "UserType": "SYSTEM",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "197895",
            "DealID": "27498",
            "Activity": "Follow-Up for Final Report on Title Notice sent to Lawyer.",
            "ActivityFrench": "Avis de suivi sur l'envoi du rapport final sur les titres � l'avocat/notaire.",
            "LogDate": "2015-08-25 21:00:12.007000000",
            "UserID": "SYSTEM",
            "UserType": "SYSTEM",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "201938",
            "DealID": "27498",
            "Activity": "Follow-Up for Final Report on Title Notice sent to Lawyer.",
            "ActivityFrench": "Avis de suivi sur l'envoi du rapport final sur les titres � l'avocat/notaire.",
            "LogDate": "2015-09-24 21:00:15.593000000",
            "UserID": "SYSTEM",
            "UserType": "SYSTEM",
            "IsShowOnLender": "true"
        },
        {
            "DealHistoryID": "202386",
            "DealID": "27498",
            "Activity": "Follow-Up for Final Report on Title Notice sent to Lawyer.",
            "ActivityFrench": "Avis de suivi sur l'envoi du rapport final sur les titres � l'avocat/notaire.",
            "LogDate": "2015-10-26 21:00:27.323000000",
            "UserID": "SYSTEM",
            "UserType": "SYSTEM",
            "IsShowOnLender": "true"
        }
    ];

    getData(): Promise<any> {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve(this.dealHistoryData);
            }, 1000);
        });
    }
}
