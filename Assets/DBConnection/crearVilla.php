<?php
include_once "cors.php";
$newVillage = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$resultado = crearVilla($newVillage);
echo json_encode($resultado);