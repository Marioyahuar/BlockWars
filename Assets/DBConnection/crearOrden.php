<?php
include_once "cors.php";
$newOrder = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$resultado = crearOrden($newOrder);
echo json_encode($resultado);