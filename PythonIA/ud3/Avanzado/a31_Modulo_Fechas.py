# main_fechas.py
import fechas

pruebas = ["15/04/2023", "32/01/2020", "10/15/2021", "hola/05/2023"]

for texto in pruebas:
    try:
        fechas.validar_fecha_dd_mm_aaaa(texto)
        print(f"{texto} es una fecha VÁLIDA.")
    except ValueError as error:
        print(f"{texto} no es válida: {error}")