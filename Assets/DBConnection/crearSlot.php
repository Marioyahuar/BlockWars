<?php
include_once "cors.php";
$newspot = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$resultado = crearSlot($newspot);
echo json_encode($resultado);