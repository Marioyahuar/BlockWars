<?php
include_once "cors.php";
$userID = $_GET["userID"];
include_once "funciones.php";
$usuario = obtenerUserName($userID);
echo json_encode($usuario);