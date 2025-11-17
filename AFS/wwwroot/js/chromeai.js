// Chrome Built-in AI API JavaScript Interop
// Provides access to Chrome's Prompt API (LanguageModel global)

let session = null;
let currentAbortController = null;

// Check if Chrome AI is available
export async function checkAvailability() {
    try {
        // Check if the AI API exists (new API uses global LanguageModel)
        if (typeof LanguageModel === 'undefined') {
            return {
                available: false,
                reason: "Chrome AI API not found. Please enable Chrome AI flags:\n" +
                    "1. chrome://flags/#prompt-api-for-gemini-nano\n" +
                    "2. chrome://flags/#optimization-guide-on-device-model\n" +
                    "Set both to 'Enabled', then restart Chrome."
            };
        }

        // Check availability using the new API
        const availability = await LanguageModel.availability();

        // Handle all possible availability states
        if (availability === 'readily' || availability === 'available') {
            return { available: true, reason: "Chrome AI is ready to use" };
        } else if (availability === 'downloadable') {
            return {
                available: false,
                reason: "Model can be downloaded.\n" +
                    "1. Visit chrome://components/\n" +
                    "2. Find 'Optimization Guide On Device Model'\n" +
                    "3. Click 'Check for update'\n" +
                    "4. Wait for download to complete\n" +
                    "5. Restart Chrome"
            };
        } else if (availability === 'after-download') {
            return {
                available: false,
                reason: "Model is downloading or needs restart.\n" +
                    "1. Wait for download to complete\n" +
                    "2. Restart Chrome\n" +
                    "If already restarted, check chrome://components/"
            };
        } else {
            return {
                available: false,
                reason: `Chrome AI is not available: ${availability}`
            };
        }
    } catch (error) {
        console.error("Chrome AI availability check error:", error);
        return {
            available: false,
            reason: `Error checking availability: ${error.message}\n\n` +
                "Make sure you have:\n" +
                "1. Chrome 127+ or Chrome Canary\n" +
                "2. Enabled chrome://flags/#prompt-api-for-gemini-nano\n" +
                "3. Enabled chrome://flags/#optimization-guide-on-device-model\n" +
                "4. Downloaded the model from chrome://components/"
        };
    }
}

// Create a new AI session
export async function createSession(progressCallback = null) {
    try {
        // Verify API is available
        if (typeof LanguageModel === 'undefined') {
            throw new Error("Chrome AI API not available. Please check flags and model download.");
        }

        // Check availability first
        const availability = await LanguageModel.availability();

        if (availability === 'no' || availability === 'unavailable') {
            throw new Error("Chrome AI model is not available on this device");
        }

        // Destroy existing session if any
        if (session) {
            try {
                session.destroy();
            } catch (e) {
                console.warn("Error destroying previous session:", e);
            }
            session = null;
        }

        // Create new abort controller for this session
        currentAbortController = new AbortController();

        // Create new session with optimal parameters for financial analysis
        const sessionOptions = {
            temperature: 0.7,  // Balanced creativity vs accuracy
            topK: 3,           // Focus on top predictions
            signal: currentAbortController.signal, // Add abort signal
        };

        // Add download progress monitor if model is downloadable
        if (availability === 'downloadable' || availability === 'after-download') {
            sessionOptions.monitor = (m) => {
                m.addEventListener('downloadprogress', (e) => {
                    console.log(`Downloaded ${e.loaded * 100}%`);
                    
                    // Call progress callback if provided
                    if (progressCallback) {
                        progressCallback(Math.round(e.loaded * 100));
                    }
                });
            };
        }

        session = await LanguageModel.create(sessionOptions);

        console.log("Chrome AI session created successfully");
        return { success: true, message: "Session created successfully" };
    } catch (error) {
        console.error("Chrome AI session creation error:", error);
        session = null;
        currentAbortController = null;
        
        if (error.name === 'AbortError') {
            return { success: false, message: "Session creation cancelled" };
        }
        
        return { success: false, message: error.message };
    }
}

// Create a new AI session with download progress monitoring
export async function createSessionWithProgress(dotnetHelper, progressCallbackMethodName) {
    try {
        // Verify API is available
        if (typeof LanguageModel === 'undefined') {
            throw new Error("Chrome AI API not available. Please check flags and model download.");
        }

        // Check availability first
        const availability = await LanguageModel.availability();

        if (availability === 'no' || availability === 'unavailable') {
            throw new Error("Chrome AI model is not available on this device");
        }

        // Destroy existing session if any
        if (session) {
            try {
                session.destroy();
            } catch (e) {
                console.warn("Error destroying previous session:", e);
            }
            session = null;
        }

        // Create new abort controller for this session
        currentAbortController = new AbortController();

        // Create new session with optimal parameters for financial analysis
        const sessionOptions = {
            temperature: 0.7,  // Balanced creativity vs accuracy
            topK: 3,           // Focus on top predictions
            signal: currentAbortController.signal, // Add abort signal
        };

        // Add download progress monitor if model is downloadable
        if (availability === 'downloadable' || availability === 'after-download') {
            console.log("Model download may be required...");
            sessionOptions.monitor = (m) => {
                m.addEventListener('downloadprogress', async (e) => {
                    const progress = Math.round(e.loaded * 100);
                    console.log(`Downloading Gemini Nano model: ${progress}%`);

                    // Notify .NET about progress
                    if (dotnetHelper && progressCallbackMethodName) {
                        try {
                            await dotnetHelper.invokeMethodAsync(progressCallbackMethodName, progress);
                        } catch (err) {
                            console.warn("Error calling .NET progress callback:", err);
                        }
                    }
                });
            };
        }

        session = await LanguageModel.create(sessionOptions);

        console.log("Chrome AI session created successfully");
        return { success: true, message: "Session created successfully" };
    } catch (error) {
        console.error("Chrome AI session creation error:", error);
        session = null;
        currentAbortController = null;
        return { success: false, message: error.message };
    }
}

// Abort current operation
export function abortCurrentOperation() {
    if (currentAbortController) {
        console.log("Aborting current operation...");
        currentAbortController.abort();
        currentAbortController = null;
        return { success: true, message: "Operation aborted" };
    }
    return { success: false, message: "No operation to abort" };
}

// Send a prompt to the AI
export async function prompt(message) {
    try {
        // Ensure session exists
        if (!session) {
            const sessionResult = await createSession();
            if (!sessionResult.success) {
                throw new Error("Failed to create session: " + sessionResult.message);
            }
        }

        // Send prompt and get response
        const response = await session.prompt(message);
        return { success: true, response: response };
    } catch (error) {
        console.error("Chrome AI prompt error:", error);

        // Try to recreate session if it was destroyed
        if (error.message.includes("session") || error.message.includes("destroy")) {
            session = null;
            return {
                success: false,
                response: null,
                error: "Session expired. Please try again."
            };
        }

        return { success: false, response: null, error: error.message };
    }
}

// Send a prompt with streaming response
export async function promptStreaming(message, dotnetHelper, callbackMethodName) {
    try {
        // Ensure session exists
        if (!session) {
            const sessionResult = await createSession();
            if (!sessionResult.success) {
                throw new Error("Failed to create session: " + sessionResult.message);
            }
        }

        // Create abort controller for this streaming operation
        currentAbortController = new AbortController();

        // Get streaming response with abort signal
        const stream = session.promptStreaming(message, {
            signal: currentAbortController.signal
        });

        // Process stream chunks
        for await (const chunk of stream) {
            if (dotnetHelper) {
                await dotnetHelper.invokeMethodAsync(callbackMethodName, chunk);
            }
        }
        
        currentAbortController = null;
        return { success: true};
    } catch (error) {
        console.error("Chrome AI streaming error:", error);
        currentAbortController = null;

        // Handle abort
        if (error.name === 'AbortError') {
            return {
                success: false,
                response: null,
                error: "Streaming cancelled by user"
            };
        }

        // Try to recreate session if it was destroyed
        if (error.message.includes("session") || error.message.includes("destroy")) {
            session = null;
            return {
                success: false,
                response: null,
                error: "Session expired. Please try again."
            };
        }

        return { success: false, response: null, error: error.message };
    }
}

// Destroy the session
export async function destroySession() {
    try {
        if (session) {
            session.destroy();
            session = null;
            console.log("Chrome AI session destroyed");
        }
        return { success: true };
    } catch (error) {
        console.error("Chrome AI session destruction error:", error);
        session = null; // Reset anyway
        return { success: false, message: error.message };
    }
}

// Clone the session (for parallel processing)
export async function cloneSession() {
    try {
        if (!session) {
            throw new Error("No active session to clone");
        }

        const clonedSession = await session.clone();
        return { success: true, session: clonedSession };
    } catch (error) {
        console.error("Chrome AI session cloning error:", error);
        return { success: false, message: error.message };
    }
}

// Get current token count
export async function getTokenCount() {
    try {
        if (!session) {
            return { success: false, count: 0, message: "No active session" };
        }

        // Get token usage information from session
        const tokensSoFar = session.tokensSoFar || 0;
        const tokensLeft = session.tokensLeft || 0;
        const maxTokens = session.maxTokens || 0;

        return {
            success: true,
            tokensSoFar: tokensSoFar,
            tokensLeft: tokensLeft,
            maxTokens: maxTokens
        };
    } catch (error) {
        console.error("Chrome AI token count error:", error);
        return { success: false, message: error.message };
    }
}

// Initialize on load
console.log("Chrome AI JavaScript module loaded");

// Provide helpful diagnostic information
if (typeof window !== 'undefined') {
    window.addEventListener('load', () => {
        setTimeout(() => {
            if (typeof LanguageModel === 'undefined') {
                console.warn(
                    "[!] Chrome AI not detected!\n\n" +
                    "To enable Chrome AI:\n" +
                    "1. Use Chrome 127+ or Chrome Canary\n" +
                    "2. Enable: chrome://flags/#prompt-api-for-gemini-nano\n" +
                    "3. Enable: chrome://flags/#optimization-guide-on-device-model\n" +
                    "4. Restart Chrome\n" +
                    "5. Download model: chrome://components/ -> 'Optimization Guide On Device Model'\n\n" +
                    "Visit: https://developer.chrome.com/docs/ai/built-in"
                );
            } else {
                LanguageModel.availability().then(availability => {
                    if (availability === 'readily' || availability === 'available') {
                        console.log("[OK] Chrome AI is ready to use!");
                    } else if (availability === 'downloadable' || availability === 'after-download') {
                        console.warn("[!] Chrome AI detected but model needs to be downloaded from chrome://components/");
                    } else {
                        console.warn(`[!] Chrome AI status: ${availability}`);
                    }
                }).catch(err => {
                    console.error("Error checking Chrome AI availability:", err);
                });
            }
        }, 1000);
    });
}
