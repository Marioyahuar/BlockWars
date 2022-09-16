<?php
include_once "cors.php";
include_once "funciones.php";
$ronda = obtenerRonda();
echo json_encode($ronda);