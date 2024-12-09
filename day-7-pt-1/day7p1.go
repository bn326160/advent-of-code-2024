package main

import (
	"fmt"
	"io"
	"log"
	"os"
	"strconv"
	"strings"
)

func evaluateExpression(numbers []int, operators []string) int {
	result := numbers[0]
	for i := 0; i < len(operators); i++ {
		if operators[i] == "+" {
			result += numbers[i+1]
		} else {
			result *= numbers[i+1]
		}
	}
	return result
}

func generateOperatorCombinations(n int) [][]string {
	if n == 0 {
		return [][]string{{}}
	}
	
	result := [][]string{}
	subCombinations := generateOperatorCombinations(n - 1)
	
	for _, sub := range subCombinations {
		plusCombo := append([]string{}, sub...)
		plusCombo = append(plusCombo, "+")
		mulCombo := append([]string{}, sub...)
		mulCombo = append(mulCombo, "*")
		
		result = append(result, plusCombo, mulCombo)
	}
	
	return result
}

func main() {
	file, err := os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer func() {
		if err = file.Close(); err != nil {
			log.Fatal(err)
		}
	}()

	b, err := io.ReadAll(file)
	if err != nil {
		log.Fatal(err)
	}
	
	inputString := string(b)
	lines := strings.Split(strings.TrimSpace(inputString), "\n")
	
	totalSum := 0
	
	for _, line := range lines {
		if line == "" {
			continue
		}
		
		parts := strings.Split(line, ": ")
		targetValue, err := strconv.Atoi(parts[0])
		if err != nil {
			log.Printf("Error parsing target value: %v", err)
			continue
		}

		numberStrs := strings.Fields(parts[1])
		numbers := make([]int, len(numberStrs))
		for i, numStr := range numberStrs {
			numbers[i], err = strconv.Atoi(numStr)
			if err != nil {
				log.Printf("Error parsing number: %v", err)
				continue
			}
		}

		// Generate all possible operator combinations
		operatorCombinations := generateOperatorCombinations(len(numbers) - 1)
		
		// Try each combination
		for _, operators := range operatorCombinations {
			result := evaluateExpression(numbers, operators)
			if result == targetValue {
				totalSum += targetValue
				break
			}
		}
	}

	fmt.Printf("Total calibration value: %d\n", totalSum)
}
