<?php
include_once "cors.php";
$serverinit = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$resultado = inicializarServer($serverinit->Id,$serverinit->TiempoInicializado);
echo json_encode($resultado);