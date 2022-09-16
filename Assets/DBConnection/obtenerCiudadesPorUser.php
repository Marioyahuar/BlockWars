<?php
include_once "cors.php";
$id = $_GET["userID"];
include_once "funciones.php";
$ciudades = obtenerCiudadesPorUsuario($id);
echo json_encode($ciudades);