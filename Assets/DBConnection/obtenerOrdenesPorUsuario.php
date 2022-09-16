<?php
include_once "cors.php";
$userID = $_GET["userID"];
include_once "funciones.php";
$misiones = obtenerOrdenesPorUsuario($userID);
echo json_encode($misiones);
